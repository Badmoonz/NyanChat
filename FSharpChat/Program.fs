module ChatClient
open System
open System.Collections.Generic;
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
    | HandShake of HandShakeMessage
    static member KnownTypes() = KnownTypes<Message>()

and 
    [<DataContract>]
    JoinMessage = { [<DataMember>] mutable Name : string;}
and 
    [<DataContract>]
    HandShakeMessage = { [<DataMember>] mutable Name : string; [<DataMember>] mutable TargetName : string; } //[<DataMember>] mutable NickName : string; [<DataMember>] mutable Avatar : string[]
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

let filterMessage name msg = 
    match msg with
    | JoinChat { Name = n; } when n <> name -> true 
    | HandShake { TargetName = tn } when tn = name -> true 
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
            let joinAction() = channel.Command <| JoinChat { Name = name;}
            do 
                channel.Open()
                channel.GetProperty<IOnlineStatus>().Online.Add(fun _ -> joinAction())
                joinAction()
            let online = List<string>();

            let rec loop () =
                async {
                    let! msg' = inbox.TryReceive(500)
                    match msg' with
                    | Some msg ->
                        if filterMessage name msg then
                            match msg with
                                | JoinChat { Name = n} ->  
                                    if (not << online.Contains) n then
                                        channel.Command <| HandShake { Name = name; TargetName = n;}
                                    channel.Command <| ChatMessage { Name = name; Message = sprintf "from %s hi ,%s " name n };
                                | HandShake { Name = n} ->
                                    if (not << online.Contains) n then
                                       online.Add n                                  
                                | LeaveChat {Name = n} -> 
                                    online.Remove n |> ignore
                                    consoleActor.Post <| sprintf "[channel : %A][%s]  %s is leaver.people left %A" chatRoom  name n online
//                                    channel.Command <| ChatMessage { Name = name; Message = sprintf "from %s bye ,%s " name n };
                                | ChatMessage {Name = n; Message = msg} ->
                                    consoleActor.Post <| sprintf "[channel : %A][%s] got from [%s] : %s" chatRoom  name n msg
                    | _ -> ()            
                    return! loop()
                }
            loop ())

    let a1 = philosopher "a1" Common 
    let a2 = philosopher "a2" Common 
    let a3 = philosopher "a3" Common 
//    philosopher "a3" (Private "azaza") |> ignore
//    philosopher "a4" (Private "azaza") |> ignore

//    Threading.Thread.Sleep(4000)
//    a1.Post <| LeaveChat {Name = "a4"}
//    a2.Post <| LeaveChat {Name = "a4"}
//    let a5 = philosopher "a5" Common
//    let a6 = philosopher "a6" Common
//    Threading.Thread.Sleep(2000)
//    a6.Post <| LeaveChat {Name = "a4"}
    Console.ReadLine() |> ignore
    0 // возвращение целочисленного кода выхода
