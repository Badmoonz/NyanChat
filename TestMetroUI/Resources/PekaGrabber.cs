
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
public static class PekaGrabber
{
	public static Dictionary<string, Lazy<BitmapImage>> PekaDict;
	static PekaGrabber()
	{
		PekaDict = new Dictionary<string, Lazy<BitmapImage>>();
		var assembly = Assembly.GetExecutingAssembly();

		PekaDict.Add(@":aws:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.aws.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":bear:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.bear.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":bitch:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.bitch.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":crazy:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.crazy.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":cry:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.cry.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":fire:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.fire.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":glory:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.glory.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":grumpy:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.grumpy.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":happy:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.happy.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":happycry:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.happycry.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":hmhm:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.hmhm.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":mad:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.mad.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":mhu:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.mhu.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":nc:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.nc.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":neponi:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.neponi.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":notch:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.notch.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":omg:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.omg.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":omsk:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.omsk.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":peka:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.peka.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":rknife:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.rknife.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":slowpoke:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.slowpoke.png");
			bi.EndInit();
			return bi;
			}));
		PekaDict.Add(@":vlast:",  new Lazy<BitmapImage>(() => 
		{
			var bi  = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = assembly.GetManifestResourceStream("TestMetroUI.Resources.Images.Pekas.vlast.png");
			bi.EndInit();
			return bi;
			}));

	}

}




