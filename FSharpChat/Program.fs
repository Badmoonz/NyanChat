module ChatClient
open System
open System.ServiceModel
open Microsoft.FSharp.Reflection 
open System.Reflection 
open System.Runtime.Serialization 
open System.Xml
 


let KnownTypes<'T>() =  
       typeof<'T>.GetNestedTypes(BindingFlags.Public ||| BindingFlags.NonPublic) 
        |> Array.filter FSharpType.IsUnion

type Agent<'T> = MailboxProcessor<'T>

[<KnownType("KnownTypes")>]
[<DataContract>]
type ChatRoomChannel = 
    | Common
    | Private of string
    static member KnownTypes() = KnownTypes<ChatRoomChannel>()

[<KnownType("KnownTypes")>]
[<DataContract>]
type Message = 
    | JoinChat of JoinMessage
    | LeaveChat of LeaveMessage
    | ChatMessage of MessageItem
    static member KnownTypes() = KnownTypes<Message>()

and 
    [<DataContract>]
    JoinMessage = { [<DataMember>] mutable Name : string}
and
    [<DataContract>]
    LeaveMessage = { [<DataMember>] mutable Name : string}
and
    [<DataContract>]
    MessageItem = { [<DataMember>] mutable Name : string; [<DataMember>] mutable Message : string}

[<ServiceContract(CallbackContract = typeof<IChatService>)>]
type IChatService = 
    [<OperationContract(IsOneWay = true)>]
    abstract member Command : msg : Message -> unit

type IChatChannel  = 
    inherit IChatService
    inherit IClientChannel


type ChatClient(agent : Agent<_>) =
    interface  IChatService with
        member __.Command(msg) = agent.Post msg

let filterMessage name = function
    | JoinChat { Name = n;  } when n <> name -> true 
    | LeaveChat { Name = n; } when n <> name -> true 
    | ChatMessage { Name = n; } when n <> name -> true 
    | _ -> false

[<EntryPoint>]
let main argv = 
    let consoleActor =
         Agent<_>.Start(fun inbox ->
            let rec loop () =
                async {
                let! msg = inbox.Receive()
                printfn "%s" msg
                return! loop()
                }
            loop ())


    let createChanel inbox channel =
        let baseAdress = @"net.p2p://chatMesh/ChatServer";
        let context = new InstanceContext(ChatClient(inbox));
        let binding = NetPeerTcpBinding("PeerTcpConfig")
        let adress =
            match channel with 
            | Common -> EndpointAddress(baseAdress)
            | Private channelName -> EndpointAddress(baseAdress + "/" + channelName)
        let f = new DuplexChannelFactory<IChatChannel>(context, binding, adress)
        //  let factory =
        //   new DuplexChannelFactory<IChatChannel>(context, "ChatEndPoint");

        f.CreateChannel()

    let rnd = Random()
    let philosopher name chatRoom =
        Agent<_>.Start(fun inbox ->
            let channel = createChanel inbox chatRoom
            do 
                channel.Open()
                channel.Command <| JoinChat { Name = name}
            let rec loop () =
                async {
                    let! msg' = inbox.TryReceive(500)
                    match msg' with
                    | Some msg ->
                        if filterMessage name msg then
                            match msg with
                                | JoinChat { Name = n} ->  
                                        channel.Command <| ChatMessage { Name = name; Message = sprintf "from %s hi ,%s " name n };
                                | LeaveChat {Name = n} -> 
                                        channel.Command <| ChatMessage { Name = name; Message = sprintf "from %s bye ,%s " name n };
                                | ChatMessage {Name = n; Message = msg} ->
                                        consoleActor.Post <| sprintf "[channel : %A][%s] got from [%s] : %s" chatRoom  name n msg
                    | _ -> ()
                    if name.EndsWith("2") &&  (rnd.Next(0,100) < 10)
                    then
                        consoleActor.Post <| sprintf "%s leaving" name
                        channel.Command <| LeaveChat {Name = name}                  
                    return! loop()
                }
            loop ())

    philosopher "a1" Common |> ignore
    philosopher "a2" Common |> ignore
//    philosopher "a3" (Private "azaza") |> ignore
//    philosopher "a4" (Private "azaza") |> ignore

    Console.ReadLine() |> ignore
    0 // возвращение целочисленного кода выхода
