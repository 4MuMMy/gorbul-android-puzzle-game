using Android.Graphics;

namespace gorbul
{
    public class r
    {

        public static string
            loadingBarR = "#ffffff",
            yukleniyorLoadingBarR = "#999999";

        public static Color
            //genel
            pencereSeffafArkaplanR = p("#99222222"),

            //yukleniyor
            yukleniyorArkaplanR = p("#222222"),
            yukleniyorTxtYaziR = p("#999999"),
            yukleniyorVersionTxtYaziR = p("#666666"),

            //anaEkran
            anaEkranGorseliArkaplanR = p("#222222"),

            //bolumler
            bolumGorseliArkaplanR = p("#222222"),
            bolumKutusuYaziR = p("#ffffff"),
            bolumKutusuArkaplanR = p("#4c8af5"),
            bolumKutusuBasildigindakiArkaplanR = p("#444444"),
            bolumKutusuBasildigindakiYaziR = p("#ffffff"),
            bolumKutusuBolumGecildiYaziR = p("#ffffff"),
            bolumKutusuBolumGecildiArkaplanR = p("#40c258"),
            bolumKutusuDeaktifYaziR = p("#999999"),
            bolumKutusuDeaktifArkaplanR = p("#bbbbbb"),

            //bulmacaEkrani
            kutuYaziR = p("#666666"),
            kutuArkaplanR = p("#bbbbbb"),
            butonDeaktifYaziR = p("#cccccc"),
            butonAktifYaziR = p("#ffffff"),
            klavyeArkaplanR = p("#cbcbcb"),
            klavyeDeaktifYaziR = p("#999999"),
            klavyeAktifYaziR = p("#666666"),
            klavyeBasildigindakiArkaplanR = p("#444444"),
            klavyeBasildigindakiYaziR = p("#ffffff"),
            kelimeAcYaziR = p("#ffffff"),
            kelimeAcArkaplanR = p("#d18432"),
            harfAlYaziR = p("#ffffff"),
            harfAlArkaplanR = p("#32afd1"),
            bolumGecYaziR = p("#ffffff"),
            bolumGecArkaplanR = p("#37d132"),
            harflerYanlisYaziR = p("#ffffff"),
            harflerYanlisArkaplanR = p("#cc1b1b"),
            kutuSeciliArkaplanR = p("#999999"),
            kutuSeciliYaziR = p("#444444"),

            soruGorseliYanEkranArkaplanR = p("#222222"),

            //menuPenceresiAc
            menuButonArkaplanR = p("#99222222"),
            menuButonTiklandiArkaplanR = p("#99666666"),
            menuSesMuzikAcikYaziR = Color.Green,
            menuSesMuzikKapaliYaziR = p("#dddddd"),

            //ayarlarPenceresiniAc
            ayarlarKodGirYaziR = p("#dddddd"),
            ayarlarKodGirBasariliYaziR = Color.Green,
            ayarlarKodGirKullanilmisYaziR = p("#dddddd"),
            ayarlarKodGirArtikGecersizYaziR = p("#dddddd"),
            ayarlarKodGirGecersizYaziR = Color.Red,

            //profilPenceresiniAc
            basarimKutusuYaziR = p("#dddddd"),
            basarimKutusuArkaplanR = p("#99222222"),
            basarimKutusuTmmlandiArkaplanR = p("#991e5e2b"),
            basarimKutusuBasarimAlindiArkaplanR = p("#990052b0"),
            basarimKutusuSolBolumArkaplanR =p("#9940c258"),

            son = Color.Transparent;

        private static Color p(string c) { return Color.ParseColor(c); }
    }
}