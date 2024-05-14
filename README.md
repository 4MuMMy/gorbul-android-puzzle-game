# Gör-Bul - Görsel Kelime Bulmaca (Visual Word Puzzle Game)<br />Source Code

<div align="center"><img src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul.jpg?raw=true" /></div>

<b>ENGLISH</b>

<h4>Gör-Bul: Visual Word Puzzle</h4>
<b>(Gör: See, Bul: Find)</b>
<br /><br />

<p>Try to find the hidden letters in the boxes by guessing what's in the picture.</p>

<p>The game has specific categories. Each category contains 40 visual puzzle sections. In the puzzle, you are shown a picture and a question. You try to guess what the correct word is by typing letters into the empty boxes on the screen. The pictures consist of specific scenes. If you can't guess the right answer within a certain time, the visual scene changes and gives you a hint. After receiving a set number of hints, you reach the final scene, and no more hints are shown. If you still struggle to find the word, you can:</p>

<p>-Use the "Buy Letter" button to open a letter in exchange for coins.</p>
<p>-Use the "Hint" button to see the visual from another angle in exchange for coins.</p>
<p>-Use the "Reveal Word" button to reveal the word and learn what the visual is in exchange for coins.</p>

<p>You can open a free chest once a day to earn coins and watch a video for an additional chest opening. Additionally, you can earn coins by watching a video from the store every 20 minutes. If you want more coins, you can purchase any amount from the store.</p>
As you solve puzzles and advance through levels, you can unlock various achievements and earn coins this way. As you progress through levels, obtaining hints becomes more challenging; the visual transition time increases. In later levels, automatic hints are no longer given, and the puzzle sections become more difficult.</p>

<p>Puzzle, Puzzle, Puzzle Adventure, Word, Word Search</p>

<br /><br /><br /><br />


<b>TURKISH:</b>
<h4>Gör-Bul: görsel kelime bulmaca</h4>
<br />

<p>Görselde ne olduğunu tahmin ederek kutulardaki gizli harfleri bulmaya çalışmaca.</p>

<p>Oyunda belirli kategoriler bulunur. Her kategorinin altında 40 adet görsel bulmaca bölümü bulunur. Bulmacada size bir görsel ile bir soru gösterilir. Ekranda bulunan boş kutulara harfleri yazarak doğru kelimenin ne olduğunu tahmin etmeye çalışırsınız. Görseller belirli sahnelerden oluşur. Eğer doğru cevabı belli bir sürede bilemezseniz, görsel sahnesi değişir ve size bir ipucu verir. Bu şekilde belirlenen sayıda ipucu aldıktan sonra son sahneye ulaşırsınız ve artık ipucu gösterilmez. Eğer kelimeyi bulmakta hâlâ zorlanıyorsanız;</p>

<p>-"Harf Al" butonu ile jeton karşılığında harf açabilirsiniz.</p>
<p>-"İpucu" butonu ile jeton karşılığında görseli başka bir açıdan görerek ipucu alabilirsiniz.</p>
<p>-"Kelime Aç" butonu ile jeton karşılığında kelimeyi açarak görselin ne olduğunu öğrenebilirsiniz.</p>

<p>Jeton kazanabilmek için günde bir defa ücretsiz sandık açabilirsiniz ve bir kez daha sandık açabilmek için video izleyebilirsiniz. Ayrıca her 20 dk'da bir mağaza üzerinden bir video seyrederek jeton kazanabilirsiniz. Daha fazla jeton sahibi olmak istiyorsanız, mağaza üzerinden istediğiniz miktarda jeton satın alabilirsiniz.</p>
<p>Bulmacaları çözerek bölüm atladıkça çeşitli başarımlar açabilirsiniz ve bu şekilde de jeton kazanabilirsiniz. Bölümleri geçtikçe ipucu alma zorlaşır; görsel atlama süresi uzar. Daha da ileriki bölümlerde artık otomatik ipucu verilmez ve bulmaca bölümleri daha çetin duruma gelir.</p>

<p>Bulmaca, Bulmaca, Bulmacalı macera, Kelime, Kelime arama</p>

<div align="center"><img src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul-icon.png?raw=true" /></div>

<pre>
Music Sources:
1.mp3 = simple-giftssimple-interludestorm-in-the-valley-from-the-simple-gifts-show-13 > https://pixabay.com/tr/music/askeri-ve-tarihi-simple-giftssimple-interludestorm-in-the-valley-from-the-simple-gifts-show-13/
2.mp3 = boismortiers-concerto-for-five-flutes-no-4-mvt-ii-allegro-36 > https://pixabay.com/tr/music/askeri-ve-tarihi-boismortiers-concerto-for-five-flutes-no-4-mvt-ii-allegro-36/
3.mp3 = children-song-happy-uplifting-funny-4432 > https://pixabay.com/tr/music/mutlu-cocuk-sarklar-children-song-happy-uplifting-funny-4432/
4.mp3 = luna-park-accordion-music-3835 > https://pixabay.com/tr/music/eglence-park-luna-park-accordion-music-3835/
5.mp3 = happy-clappy-23 > https://pixabay.com/tr/music/mutlu-cocuk-sarklar-happy-clappy-23/
6.mp3 = victory-march-3172 > https://pixabay.com/tr/music/eglence-park-victory-march-3172/
</pre>

<pre>
  NUGET

Xamarin.Forms
Newtonsoft.Json
SHA3.Net
Xamarin.Android.Google.BillingClient
Xamarin.Android.Support.Design
Xamarin.Google.Android.Material
Xamarin.GooglePlayServices.Ads
Xamarin.GooglePlayServices.Auth
Xamarin.GooglePlayServices.Games


USING

using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Collections;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.OS;
using Android.Gms.Ads;
using Android.BillingClient.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Tasks;
using Android.Util;
using Android.Runtime;
using Android.Media;

using Java.Lang;
using Java.Security;
using Java.Security.Spec;
using Java.IO;
using Java.Interop;

using Newtonsoft.Json;
using SHA3.Net;
</pre>

<h5>BAŞARIMLAR - ACHIEVEMENTS</h5>
<pre>
Başlık
Açıklama
Ödül
Hedef Değer


En zor adım, ilk adım!
1 bulmaca çözün.
5
1


Üçüncü bulmacanı da çözdün!
3 bulmaca çözün.
5
3


Onda on!
10 bulmaca çözün.
10
10


Bölüm 1: Hayvanlar
Bölüm 1: Hayvanlar bölümünü tamamlayın.
40
10


Ellisi de ellisii!
50 bulmaca çözün.
15
50


Bölüm 2: Bitkiler
Bölüm 2: Bitkiler bölümünü tamamlayın.
80
10


Yüz soru, yüz cevap!
100 bulmaca çözün.
20
100

</pre>

<h5>PHOTO SOURCES</h5>
<pre>
  https://pixabay.com/tr/photos/fil-safari-hayvan-savunma-afrika-1421167/
https://pixabay.com/tr/photos/hayvan-at-s%C4%B1pa-evcil-hayvan-ati-3281017/
https://pixabay.com/tr/photos/ku%C5%9F-durian-dragon-%C3%A7ilek-kanatlar-1045954/
https://pixabay.com/tr/photos/ay%C4%B1-brown-bear-hayvanat-bah%C3%A7esi-2747135/
https://pixabay.com/tr/photos/honey-bee-b%C3%B6cek-hayvan-besleme-4314838/
https://pixabay.com/tr/photos/kaz-vah%C5%9Fi-kaz-su-ku%C5%9Fu-ku%C5%9F-3477674/
https://pixabay.com/tr/photos/hammond-koyun-ram-wilderness-erkek-3576440/
https://pixabay.com/tr/photos/m%C3%BCh%C3%BCr-robbe-burun-ayak-di%C5%9F-a%C3%A7%C4%B1k-4346802/
https://pixabay.com/tr/photos/kedi-yavru-kedi-evcil-hayvanlar-2934720/
https://pixabay.com/tr/photos/inek-manzara-%C3%A7imen-alan-k%C4%B1r-2132526/
https://pixabay.com/tr/photos/hayvan-d%C3%BCnya-deve-tay-gen%C3%A7-deve-5002821/
https://pixabay.com/tr/photos/leopar-leopard-spotlar-hayvan-592187/
https://pixabay.com/tr/photos/ke%C3%A7i-hayvan-memeli-yerli-ke%C3%A7i-5535783/
https://pixabay.com/tr/photos/saka-ku%C5%9Fu-ku%C5%9F-do%C4%9Fa-gaga-kanatlar%C4%B1-4232130/
https://pixabay.com/tr/photos/ah%C5%9Fap-fare-kemirgen-nager-yem-fare-3082922/
https://pixabay.com/tr/photos/kuzu-hayvan-kafa-y%C3%BCz-koyun-5972583/
https://pixabay.com/tr/photos/wolf-inleyen-hayvan-yabani-do%C4%9Fa-1992716/
https://pixabay.com/tr/photos/e%C5%9Fek-hayvan-mera-memeli-otlak-5998885/
https://pixabay.com/tr/photos/%C3%A7ok-hayvanat-bah%C3%A7esi-lama-memeli-2240256/
https://pixabay.com/tr/photos/%C3%A7ita-y%C4%B1rt%C4%B1c%C4%B1-hayvan-afrika-3475778/
https://pixabay.com/tr/photos/swan-ku%C5%9F-su-ku%C5%9Flar%C4%B1-beyaz-ku%C4%9Fu-359931/
https://pixabay.com/tr/photos/bal%C4%B1k-carassius-pe%C3%A7e-kuyru%C4%9Fu-palet-5917864/
https://pixabay.com/tr/photos/panda-k%C3%B6pek-yavrusu-ay%C4%B1-%C5%9Firin-%C3%A7in-4478090/
https://pixabay.com/tr/photos/rakun-hayvan-orman-memeli-5673543/
https://pixabay.com/tr/photos/zebra-hayvan-safari-at-memeli-163052/
https://pixabay.com/tr/photos/horoz-t%C3%BCyler-t%C3%BCyleri-tepe-5544252/
https://pixabay.com/tr/photos/alageyik-erkeklere-%C3%B6zel-geyik-984573/
https://pixabay.com/tr/photos/hayvan-d%C3%BCnya-do%C4%9Fa-memeli-hayvan-3176625/
https://pixabay.com/tr/photos/goril-maymun-hayvan-3606721/
https://pixabay.com/tr/photos/lemur-maki-catta-kuyruk-%C3%A7imen-2441652/
https://pixabay.com/tr/photos/fauna-flora-domuz-eti-pembe-do%C4%9Fa-2713066/
https://pixabay.com/tr/photos/y%C4%B1lan-top-python-y%C4%B1lan%C4%B1-s%C3%BCr%C3%BCngen-4631379/
https://pixabay.com/tr/photos/peregrine-falcon-%C5%9Fahin-y%C4%B1rt%C4%B1c%C4%B1-ku%C5%9F-3023839/
https://pixabay.com/tr/photos/g%C3%BCm%C3%BC%C5%9F-destekli-%C3%A7akal-uzun-kuyruk-4653964/
https://pixabay.com/tr/photos/ku%C5%9F-yaban-hayat%C4%B1-hindi-vah%C5%9Fi-hindi-6202055/
https://pixabay.com/tr/photos/aslan-kedi-etobur-hayvan-5873637/
https://pixabay.com/tr/photos/habe%C5%9F-maymunu-maymun-primat-yarat%C4%B1k-3089012/
https://pixabay.com/tr/photos/tapir-g%C3%BCney-amerika-hayvan-memeli-4155997/
https://pixabay.com/tr/photos/fox-yaban-hayat%C4%B1-hayvan-kaya%C3%A7lar-1031632/
https://pixabay.com/tr/photos/yunuslar-akvaryum-atlama-bal%C4%B1k-906165/
https://pixabay.com/tr/photos/polonya-tatry-da%C4%9Flar-manzara-do%C4%9Fa-3400440/
https://pixabay.com/tr/photos/%c3%a7im-bi%c3%a7me-makinesi-%c3%a7im-kesme-938555/
https://pixabay.com/tr/photos/pembe-%c3%a7i%c3%a7ek-pembe-g%c3%bcl-g%c3%bclnihal-3475452/
https://pixabay.com/tr/photos/meyve-%c3%bcz%c3%bcm-%c5%9farap-ba%c4%9f-asma-3215625/
https://pixabay.com/tr/photos/arpa-hububat-%c3%bcr%c3%bcnleri-kulak-tar%c4%b1m-5233734/
https://pixabay.com/tr/photos/nane-bitki-ye%c5%9fil-5091575/
https://pixabay.com/tr/photos/incir-g%c4%b1da-yabani-meyve-yaprak-3362472/
https://pixabay.com/tr/photos/m%c4%b1s%c4%b1r-bitki-tar%c4%b1m-g%c4%b1da-do%c4%9fa-alan-5463051/
https://pixabay.com/tr/photos/sebze-havu%c3%a7-g%c4%b1da-maliyeti-yemek-5321670/
https://pixabay.com/tr/photos/pamuk-yeti%c5%9ftirme-istanbul-bitkiler-223736/
https://pixabay.com/tr/photos/bambu-orman-ye%c5%9fil-bitki-wood-asya-20936/
https://pixabay.com/tr/photos/rock-deniz-yosunu-deniz-plaj-su-3526900/
https://pixabay.com/tr/photos/cereus-ye%c5%9fil-barajlar-needles-1453793/
https://pixabay.com/tr/photos/ananas-%c3%a7iftlik-bah%c3%a7e-do%c4%9fa-bitki-3664499/
https://pixabay.com/tr/photos/orkide-phalaenopsis-%c3%a7i%c3%a7ek-4773/
https://pixabay.com/tr/photos/karpuz-meyve-horta-yeşiller-351388
https://pixabay.com/tr/photos/meyve-do%c4%9fa-levha-sa%c4%9fl%c4%b1kl%c4%b1-g%c4%b1da-3313970/
https://pixabay.com/tr/photos/mangetout-bezelye-bitki-g%c4%b1da-ye%c5%9fil-2416152/
https://pixabay.com/tr/photos/avokado-a%c4%9fa%c3%a7-bitki-defne-sera-454287/
https://pixabay.com/tr/photos/kestane-sonbahar-kestane-meyve-3673443/
https://www.pexels.com/tr-tr/fotograf/patates-144248/
https://pixabay.com/tr/photos/%C3%A7in-judas-a%C4%9Fac%C4%B1-a%C4%9Fa%C3%A7-%C3%A7i%C3%A7ek-%C3%A7i%C3%A7e%C4%9Fi-1367913/
https://pixabay.com/tr/photos/linde-limon-%c3%a7i%c3%a7e%c4%9fi-ihlamur-a%c4%9fa%c3%a7-5350285/
https://www.pexels.com/tr-tr/fotograf/hindistan-cevizi-agaclarinin-dusuk-acili-fotografciligi-804410/
https://pixabay.com/tr/photos/%c3%a7i%c3%a7ek-mor-lavanta-do%c4%9fa-bah%c3%a7e-5383054/
https://pixabay.com/tr/photos/marguerit-beyaz-%c3%a7i%c3%a7ek-papatya-5959944/
https://www.pexels.com/tr-tr/fotograf/kirmizi-meyveler-574899/
https://pixabay.com/tr/photos/patl%c4%b1can-sebze-su-damlac%c4%b1klar%c4%b1-5668318/
https://www.pexels.com/tr-tr/fotograf/gida-saglikli-vejetaryen-fabrika-4198112/
https://pixabay.com/tr/photos/greyfurt-meyve-narenciye-vitaminler-4396202/
https://pixabay.com/tr/photos/sar%c4%b1msak-bile%c5%9fen-lezzet-baharat-3747176/
https://pixabay.com/tr/photos/k%c3%b6k-zencefil-sa%c4%9fl%c4%b1kl%c4%b1-617409/
https://pixabay.com/tr/photos/ay%c3%a7i%c3%a7e%c4%9fi-sar%c4%b1-%c3%a7i%c3%a7ek-polen-1627193/
https://pixabay.com/tr/photos/kabak-sebzeler-bitki-sa%c4%9fl%c4%b1kl%c4%b1-taze-3649232/
https://pixabay.com/tr/photos/a%c4%9fa%c3%a7-ivy-g%c3%bcne%c5%9fli-g%c3%bcn-dinlenmek-2673665/
https://pixabay.com/tr/photos/haşhaş-çiçek-kırmızı-haşhaş-3438668
https://pixabay.com/tr/photos/han%c4%b1meli-bah%c3%a7e-han%c4%b1meli-3493314/
https://pixabay.com/tr/photos/bitki-%c3%b6rt%c3%bcs%c3%bc-kardelen-bahar-%c3%a7i%c3%a7ek-4181386/
https://pixabay.com/tr/photos/sardunya-geranium-pembe-pink-bitki-5005159/
https://pixabay.com/tr/photos/ortak-b%c3%b6%c4%9f%c3%bcrtlen-b%c3%b6%c4%9f%c3%bcrtlen-%c3%a7al%c4%b1-1687348/
https://pixabay.com/tr/photos/alba-morus-dut-beyaz-meyve-88456/
</pre>

<h5>SCORE TABLES</h5>
<pre>
  Başlık
ID

[1-40] Ortalaması En Yüksek Puanlar


[41-200] Ortalaması En Yüksek Puanlar


[201-400] Ortalaması En Yüksek Puanlar


[401-600] Ortalaması En Yüksek Puanlar


[601-800] Ortalaması En Yüksek Puanlar


[801-1000] Ortalaması En Yüksek Puanlar
</pre>

<img width="300px" src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul1.png?raw=true" /><img width="300px"  src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul2.png?raw=true" /><img width="300px"  src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul3.png?raw=true" /><img width="300px"  src="https://github.com/4MuMMy/gorbul-android-puzzle-game/blob/main/gorbul4.png?raw=true" />
