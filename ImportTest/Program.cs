// See https://aka.ms/new-console-template for more information
using ZUEPC.Import.Parser;

public class Program
{
	static void Main()
	{
		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\AFC hromadné 13x.xml");
		//XDocument doc = XDocument.Load(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\moj_testovaci.xml");
		//ImportCREPC();
		//string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\AFC hromadné 13x.xml");
		//string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\ADC_s_ohlasmi_my.xml");
		//string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z CREPČ2\Testovacie\ADC_samostatne.xml");
		//ImportParser.ManualParseCREPC(docString);

		//string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z DaWinci\Testovacie\ADC_s_ohlasmi.ISO");
		//string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z DaWinci\Testovacie\ADC_samostatne.ISO");
		string docString = File.ReadAllText(@"D:\Skola\Inzinier\Diplomova_praca\Material_k_systemu\Informačný systém Publikačná činnosť UNIZA\Exporty XML\z DaWinci\Testovacie\ADC_hromadne_10x.ISO");
		ImportParser.ManualParseDaWinci(docString);
	}
}