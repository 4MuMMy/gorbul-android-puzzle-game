<?php
//REST API
date_default_timezone_set('Europe/Istanbul');
$conn = new mysqli('localhost:3307', 'root', '', 'gorbul');
$conn->set_charset("utf8");
if ($conn->connect_error) {
	die("errcode31" /*$conn->connect_error*/);//veritabanına bağlanırken hata oluştu
}


$serverTimeRequest=(isset($_GET['st']) ? $_GET['st'] : null);
$gelenToken=(isset($_GET['t']) ? $_GET['t'] : null);

use \Datetime;
$now = new DateTime("now", new DateTimeZone("UTC"));

if ($serverTimeRequest=="e"){
	
	//şu anki timeı cihaza gönderiyoruz
	
	$ts=$now->format("Y-m-d H:i:s");
	
	//ufak bir şifreleme yapıyoruz
	$ts=str_replace("0","*",$ts);
	$ts=str_replace("1","/",$ts);
	$ts=str_replace("2","{",$ts);
	$ts=str_replace("3","+",$ts);
	$ts=str_replace("4","_",$ts);
	$ts=str_replace("5","!",$ts);
	$ts=str_replace("6","%",$ts);
	$ts=str_replace("7","(",$ts);
	$ts=str_replace("8",")",$ts);
	$ts=str_replace("9","#",$ts);
	$ts=str_replace("-","$",$ts);
	$ts=str_replace(" ","]",$ts);
	$ts=str_replace(":","½",$ts);
	
	echo($ts);
	
}
else{



	//LOG KAYIT ______________________ bu bölüm geliştirme dışında devre dışı bırakılmalı TOKEN DEAKTİF

	if (!empty($_POST))
	{
	header('Content-Type: application/x-www-form-urlencoded');

	$logMu=(isset($_POST['_l']) ? $_POST['_l'] : null);

	if ($logMu=="o")
	{
		$log=(isset($_POST['lg']) ? $_POST['lg'] : null);
		
		if ($logEkle=$conn->prepare("insert into log(txt) values(?)")){
			$logEkle->bind_param("s", $log);
			$logEkle->execute();
			$logEkle->close();
		}
		die("ok");
	}


	}

	//LOG KAYIT_____________________

	$clientVersion="";


	if ($gelenToken==null) {
		$gelenToken=(isset($_POST['t']) ? $_POST['t'] : null);
	}

	if ($gelenToken==null) {
		die("yok");
	}

	//version kısmını ayırıyoruz
	if (!(strpos($gelenToken,"_v")===false)){
		$clientVersion=explode("_v",$gelenToken)[1];
		
		//fake token kısmını ayırıyoruz ve gerçek token kısmını alıyoruz
		if (strlen($gelenToken)>=82)
			$gelenToken=substr($gelenToken,0,64);
		else die("yok");
	}
	else die("yok");

	

	//token oluşturuyoruz
	$tokenOk=false;
	$saniyeEsnetmeSiniri=8;//token kaç saniye beklendikten sonra iptal edilecek
	$saniyeEsnet=0;

	while (true){

		if ($saniyeEsnet!=0) $now->modify("-1 seconds");
		
		$yil = intval($now->format("Y"));
		$ay = intval($now->format("n"));
		$gun = intval($now->format("j"));
		$saat = intval($now->format("G"));
		$dk = intval($now->format("i"));
		$sn = intval($now->format("s"));

		//kolay bulunmasını zorlaştırmak için şu anki verilere belirli sayılar ekliyoruz
		$yil += 561701840414;
		$ay += 17862422145;
		$gun += 586121244;
		$saat += 8944542165;
		$dk += 48412154256;
		$sn += 87684254125;

		$hepsininToplami = $yil + $ay + $gun + $saat + $dk + $sn;
		
		$karistir = $hepsininToplami . "." . $gun . "-" . $dk . "/" . $yil . ":" . $saat . ";" . $sn . "\\" . $ay;

		$token = hash("sha256", $karistir . "tknbymmyv1");

		if ($token==$gelenToken){
			$tokenOk=true;
			break;
		}
		else{
			$saniyeEsnet++;
			if ($saniyeEsnet>=$saniyeEsnetmeSiniri){
				break;
			}
		}
	}


	

	$reklamBeklemeSuresiDK=20;
	$sandikBeklemeSuresiSA=24;
	$magazaReklamVerilecekJeton=2;
	$harfAlJetonTutari=6;
	$ipucuJetonTutari=10;
	$kelimeAcJetonTutari=20;


	//token kontrolü yapıyoruz
	if (!$tokenOk){
		echo("yok");//token geçerli değilse
	}
	else{
		//token geçerliyse devam

			$htx=(isset($_GET['h']) ? $_GET['h'] : null);

			//görsel isteğini işliyoruz bölüm görselleri: _b_/-token-/1.jpg
			if ($htx=="t"){
				$dosya=(isset($_GET['g']) ? $_GET['g'] : null);
				if ($dosya!=null){
					
					$name = "_b_/".$dosya;
					$fp = fopen($name, 'rb');

					header("Content-Type: image/jpg");
					header("Content-Length: ".filesize($name));

					fpassthru($fp);
					exit;
					
				}
			}
			//görsel isteğini işliyoruz soru görselleri: _i_/-token-/1.jpg
			else if ($htx=="t2"){
				$klasor=(isset($_GET['k']) ? $_GET['k'] : null);
				if ($klasor!=null){
					$dosya=(isset($_GET['g']) ? $_GET['g'] : null);
					if ($dosya!=null){
						
						$name = "_i_/".$klasor."/".$dosya;
						$fp = fopen($name, 'rb');

						header("Content-Type: image/jpg");
						header("Content-Length: ".filesize($name));

						fpassthru($fp);
						exit;
						
					}
				}
			}
			else{

			//uygulama aktiflik kontrolü -- eğer uygulama aktif değilse oynayan oyuncuları istek geldiğinde ana ekrana döndür

			$aktifMi__="";
			$version__="";

			$aktifCek=$conn->prepare("select aktif,version from ayarlar limit 1");
			$aktifCek->execute();
			$aktifCek=$aktifCek->get_result();
			if($_scx = mysqli_fetch_assoc($aktifCek)){
				$aktifMi__=$_scx["aktif"];
				$version__=$_scx["version"];
			}
			
			
			
			//aktifse devam
			if ($aktifMi__=="1"){
				//version aynı ise devam
				if ($clientVersion==$version__){

				if (empty($_POST))
				{
					$durum=(isset($_GET['_d']) ? $_GET['_d'] : null);
					
						
					if ($durum=="a")//ayarları çek
					{
						$return="";
						$ayarlariCek=$conn->prepare("select duyuru from ayarlar limit 1");
						$ayarlariCek->execute();
						$ayarlariCek=$ayarlariCek->get_result();
						if($_sc = mysqli_fetch_assoc($ayarlariCek)){
							$return.=
							'{"aktif":"'.$aktifMi__.'",'.
							'"duyuru":"'.$_sc["duyuru"].'",'.
							'"version":"'.$version__.'"}';
						}
						echo($return);
					}
					else if ($durum=="b")//ban kontrolü ve oyuncu ayarları
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$return="";
						$banKontrol=$conn->prepare("select id from yasaklilar where googleID=? limit 1");
						$banKontrol->bind_param("s", $googleID);
						$banKontrol->execute();
						$banKontrol=$banKontrol->get_result();
						if($_sc = mysqli_fetch_assoc($banKontrol)){
							$return="e";
						}
						
						//banlı değilse
						if ($return=="") {
							//oyuncu ayarlarını çekiyoruz
							$return="";
							$ayarlariCek=$conn->prepare("select * from oAyarlari where googleID=? limit 1");
							$ayarlariCek->bind_param("s", $googleID);
							$ayarlariCek->execute();
							$ayarlariCek=$ayarlariCek->get_result();
							if($_sc = mysqli_fetch_assoc($ayarlariCek)){
								$return.=
								'{"sesler":"'.($_sc["sesler"]=="1"?"True":"False").'",'.
								'"muzik":"'.($_sc["muzik"]=="1"?"True":"False").'",'.
								'"titresim":"'.($_sc["titresim"]=="1"?"True":"False").'",'.
								'"yanlisKelimeSil":"'.($_sc["yanlisKelimeSil"]=="1"?"True":"False").'"}';
							}
						}
						
						echo($return);
					}
					else if ($durum=="tml")//temel verileri çek: bölümler, sorular, başarımlar, skor tabloları
					{
						$bolumler_R="";
						$bolumleriCek=$conn->prepare("select * from bolumler order by id");
						$bolumleriCek->execute();
						$bolumleriCek=$bolumleriCek->get_result();
						while($_sc = mysqli_fetch_assoc($bolumleriCek)){
							$bolumler_R.=
							'{"id":"'.$_sc["id"].'",'.
							'"bolumAdi":"'.$_sc["bolumAdi"].'"},';
						}
						if ($bolumler_R!="") $bolumler_R='['.substr($bolumler_R, 0, -1).']';
						
						if ($bolumler_R==""){
							echo("");
							return;
						}
						
						$sorular_R="";
						$sorulariCek=$conn->prepare("select * from sorular order by id");
						$sorulariCek->execute();
						$sorulariCek=$sorulariCek->get_result();
						while($_sc = mysqli_fetch_assoc($sorulariCek)){
							$sorular_R.=
							'{"id":"'.$_sc["id"].'",'.
							'"soru":"'.$_sc["soru"].'",'.
							'"cevap":"'.$_sc["cevap"].'",'.
							'"gorselSayisi":"'.$_sc["gorselSayisi"].'"},';
						}
						if ($sorular_R!="") $sorular_R='['.substr($sorular_R, 0, -1).']';
						
						if ($sorular_R==""){
							echo("");
							return;
						}
						
						$basarimlar_R="";
						$basarimCek=$conn->prepare("select * from basarimlar order by id");
						$basarimCek->execute();
						$basarimCek=$basarimCek->get_result();
						while($_sic = mysqli_fetch_assoc($basarimCek)){
							$basarimlar_R.=
							'{"id":"'.$_sic["id"].'",'.
							'"baslik":"'.$_sic["baslik"].'",'.
							'"aciklama":"'.$_sic["aciklama"].'",'.
							'"odul":"'.$_sic["odul"].'",'.
							'"hedefDeger":"'.$_sic["hedefDeger"].'",'.
							'"achi_id":"'.$_sic["achi_id"].'"},';
						}
						if ($basarimlar_R!="") $basarimlar_R='['.substr($basarimlar_R, 0, -1).']';
						
						if ($basarimlar_R==""){
							echo("");
							return;
						}
						
						$skorTablolari_R="";
						$skorTabloCek=$conn->prepare("select * from skorTablolari order by id");
						$skorTabloCek->execute();
						$skorTabloCek=$skorTabloCek->get_result();
						while($_sic = mysqli_fetch_assoc($skorTabloCek)){
							$skorTablolari_R.=
							'{"id":"'.$_sic["id"].'",'.
							'"ilk":"'.$_sic["ilk"].'",'.
							'"son":"'.$_sic["son"].'",'.
							'"lead_id":"'.$_sic["lead_id"].'"},';
						}
						if ($skorTablolari_R!="") $skorTablolari_R='['.substr($skorTablolari_R, 0, -1).']';
						
						if ($skorTablolari_R==""){
							echo("");
							return;
						}
						
						$ayrac="___";
						
						echo($bolumler_R . $ayrac . $sorular_R . $ayrac . $basarimlar_R . $ayrac . $skorTablolari_R);
					}
					else if ($durum=="jt")//jeton bilgilerini çek
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$return="";
						$jetonlariCek=$conn->prepare("select * from jetonlar where googleID=? limit 1");
						$jetonlariCek->bind_param("s", $googleID);
						$jetonlariCek->execute();
						$jetonlariCek=$jetonlariCek->get_result();
						if($_sc = mysqli_fetch_assoc($jetonlariCek)){
							$return.=
							'{"id":"'.$_sc["id"].'",'.
							'"jetonSayisi":"'.$_sc["jetonSayisi"].'",'.
							'"sonJetonTarihi":"'.$_sc["sonJetonTarihi"].'",'.
							'"gunlukJeton":"'.$_sc["gunlukJeton"].'"}';
						}
						echo($return);
					}
					else if ($durum=="hjt")//jeton kontrolü ve jeton harcama
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$tur=(isset($_GET['_j']) ? $_GET['_j'] : null);
						
						$jetonSayisi=0;
						
						if ($tur=="hal")
							$harcanacakJeton=$harfAlJetonTutari;
						else if ($tur=="ipg")
							$harcanacakJeton=$ipucuJetonTutari;
						else if ($tur=="kla")
							$harcanacakJeton=$kelimeAcJetonTutari;
						
						$return="";
						$jetonlariCek=$conn->prepare("select jetonSayisi from jetonlar where googleID=? limit 1");
						$jetonlariCek->bind_param("s", $googleID);
						$jetonlariCek->execute();
						$jetonlariCek=$jetonlariCek->get_result();
						if($_sc = mysqli_fetch_assoc($jetonlariCek)){
							$jetonSayisi=$_sc["jetonSayisi"];
						}
						
						//yeterli jeton varsa
						if ($harcanacakJeton<=$jetonSayisi){

							//jeton eksiltiyoruz
							if ($jetonHarca=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi-? where googleID=?")){
								$jetonHarca->bind_param("ss", $harcanacakJeton, $googleID);
								$jetonHarca->execute();
								$jetonHarca->close();
							}
							$return="ok";
						}
						else{
							//yeterli jeton yoksa
							$return="yjy_".$jetonSayisi;
						}
						
						
						echo($return);
					}
					else if ($durum=="ssc")//soru istatistik idlerini, bolumgecildi ve toplamPuan çek, bölümleri oluşturmak ve play skor için
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						if ($googleID!=null){
							$return="";
							$soruIstatistikCek=$conn->prepare("select soruID,bolumGecildi,toplamPuan from soruIstatistik where googleID=? order by soruID");
							$soruIstatistikCek->bind_param("s" ,$googleID);
							$soruIstatistikCek->execute();
							$soruIstatistikCek=$soruIstatistikCek->get_result();
							while($_sic = mysqli_fetch_assoc($soruIstatistikCek)){
								$return.=
								'{"soruID":"'.$_sic["soruID"].'",'.
								'"bolumGecildi":"'.($_sic["bolumGecildi"]=="1"?"True":"False").'",'.
								'"toplamPuan":"'.$_sic["toplamPuan"].'"},';
							}
							if ($return!="") $return='['.substr($return, 0, -1).']';
							echo($return);
						}
					}
					else if ($durum=="sict")//tek bir soru istatistik kaydı çek
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$soruID=(isset($_GET['_sid']) ? $_GET['_sid'] : null);
						
						if ($googleID!=null && $soruID!=null){
							$return="";
							$tekSoruIstatistikCek=$conn->prepare("select * from soruIstatistik where googleID=? and soruID=? limit 1");
							$tekSoruIstatistikCek->bind_param("ss" ,$googleID, $soruID);
							$tekSoruIstatistikCek->execute();
							$tekSoruIstatistikCek=$tekSoruIstatistikCek->get_result();
							if($_sic = mysqli_fetch_assoc($tekSoruIstatistikCek)){
								$return.=
								'{"soruID":"'.$_sic["soruID"].'",'.
								'"bolumGecildi":"'.($_sic["bolumGecildi"]=="1"?"True":"False").'",'.
								'"hangiHarflerAcildi":"'.$_sic["hangiHarflerAcildi"].'",'.
								'"toplamSure":"'.$_sic["toplamSure"].'",'.
								'"yanlisDenemeSayisi":"'.$_sic["yanlisDenemeSayisi"].'",'.
								'"gorselSahneSayisi":"'.$_sic["gorselSahneSayisi"].'",'.
								'"suAnkiGorselSahneSuresi":"'.$_sic["suAnkiGorselSahneSuresi"].'",'.
								'"uzaklastirmaSayisi":"'.$_sic["uzaklastirmaSayisi"].'",'.
								'"harfAlmaSayisi":"'.$_sic["harfAlmaSayisi"].'",'.
								'"kelimeAcildi":"'.($_sic["kelimeAcildi"]=="1"?"True":"False").'",'.
								'"toplamPuan":"'.$_sic["toplamPuan"].'",'.
								'"acilanHarfler":"'.$_sic["acilanHarfler"].'"}';
							}
							echo($return);
						}
					}
					else if ($durum=="obg")//önceki bölümler geçilmiş mi kontrolü
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$soruID=(isset($_GET['_sid']) ? $_GET['_sid'] : null);
						
						if ($googleID!=null && $soruID!=null){
							
							$oncekiBolumlerGecilmis=1;
							
							//şimdiki sorudan düşük olan tüm idleri sorguluyoruz ve hepsinin bölüm geçilmiş olduğuna bakıyoruz
							$bolumGecildiKontrol=$conn->prepare("select bolumGecildi from soruIstatistik where googleID=? and soruID<? order by soruID");
							$bolumGecildiKontrol->bind_param("ss" ,$googleID, $soruID);
							$bolumGecildiKontrol->execute();
							$bolumGecildiKontrol=$bolumGecildiKontrol->get_result();
							while($_sic = mysqli_fetch_assoc($bolumGecildiKontrol)){
								//eğer geçilmeyen bir bölüm varsa direk çık
								if ($_sic["bolumGecildi"]=="0"){
									$oncekiBolumlerGecilmis=0;
									break;
								}
							}
							
							$return="1";
							
							//önceki bölümlerde geçilmemiş olan varsa 0 döndür, sorun yoksa 1
							if ($oncekiBolumlerGecilmis==0) $return="0";
							
							echo($return);
						}
					}
					else if ($durum=="kd")//kod girme işlemlerini yap ve sonucu göster
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$girilenKod=(isset($_GET['kd']) ? $_GET['kd'] : null);
						
						if ($googleID!=null && strlen($girilenKod)==8){
							$kod="";
							$bitisTarihi="";
							$odulJeton=0;
							
							$return="";
							
							$kodGecerliMi=$conn->prepare("select * from kodlar where kod=? limit 1");
							$kodGecerliMi->bind_param("s", $girilenKod);
							$kodGecerliMi->execute();
							$kodGecerliMi=$kodGecerliMi->get_result();
							if($_sc = mysqli_fetch_assoc($kodGecerliMi)){
								$kod=$_sc["kod"];
								$odulJeton=$_sc["odulJeton"];
								$bitisTarihi=$_sc["bitisTarihi"];
							}
							
							if ($kod!=""){
								$simdikiTarih=date("Y-m-d H:i:s");
									
								if (strtotime($bitisTarihi)>=strtotime($simdikiTarih)){
									//echo("Bu kod geçerli. (Son Bulacak: ".$bitisTarihi.")");
									
									
									$kodKullanilmadi=true;
									$kodKullanildiMi=$conn->prepare("select id from kullanilanKodlar where kullanilanKod=? and googleID=? limit 1");
									$kodKullanildiMi->bind_param("ss", $kod, $googleID);
									$kodKullanildiMi->execute();
									$kodKullanildiMi=$kodKullanildiMi->get_result();
									if($_sc = mysqli_fetch_assoc($kodKullanildiMi)){
										$kodKullanilmadi=false;
									}
									
									if ($kodKullanilmadi){
										//kod daha önce bu oyuncu tarafından kullanılmamış
										
										//kodu kullanıyoruz
										
										//ödül jetonları veriyoruz
										if ($koduKullan=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+? where googleID=?")){
											$koduKullan->bind_param("ss", $odulJeton, $googleID);
											$koduKullan->execute();
											$koduKullan->close();
										}
										
										//bu oyuncu için kodu kullanıldı olarak ekliyoruz
										if ($kodKullanildi=$conn->prepare("insert into kullanilanKodlar(googleID,kullanilanKod,kullanilanTarih) values(?,?,?)")){
											$kodKullanildi->bind_param("sss", $googleID, $kod, $simdikiTarih);
											$kodKullanildi->execute();
											$kodKullanildi->close();
										}
										
										//$return="Kodu başarı ile kullandınız. Jetonlarınız güncellendi.";
										$return="1";
									}
									else{
										//$return="Bu kodu zaten daha önce kullandınız.";
										$return="2";
									}
									
								}
								else {
									//$return="Bu kod artık geçerli değil. (Son Buldu: ".$bitisTarihi.")";
									$return="3";
								}
							}
							else $return="4";//$return="Kod geçerli değil.";
							
							echo($return);
						}
					}
					else if ($durum=="is")//ilerleyişi sıfırla
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$return="";
						
						//tüm ilerleyişi sıfırlıyoruz
						if ($ilerleyisSifirla=$conn->prepare("delete soruIstatistik,jetonlar,basarimKontrol,kullanilanKodlar,oAyarlari from soruIstatistik left join jetonlar on (jetonlar.googleID=soruIstatistik.googleID) left join basarimKontrol on (basarimKontrol.googleID=soruIstatistik.googleID) left join kullanilanKodlar on (kullanilanKodlar.googleID=soruIstatistik.googleID) left join oAyarlari on (oAyarlari.googleID=soruIstatistik.googleID) where soruIstatistik.googleID=?")){
							$ilerleyisSifirla->bind_param("s", $googleID);
							$ilerleyisSifirla->execute();
							$ilerleyisSifirla->close();
						}
						
						//silinmemiş olma ihtimali oluşuyor NEDENSE? tekrar silmeleri tek tek yapıyoruz
						
						
						//tüm soruları & bölümleri sıfırlıyoruz
						if ($bolumSifirla=$conn->prepare("delete from soruIstatistik where googleID=?")){
							$bolumSifirla->bind_param("s", $googleID);
							$bolumSifirla->execute();
							$bolumSifirla->close();
						}
						
						//jetonları sıfırlıyoruz
						if ($jetonSifirla=$conn->prepare("delete from jetonlar where googleID=?")){
							$jetonSifirla->bind_param("s", $googleID);
							$jetonSifirla->execute();
							$jetonSifirla->close();
						}
						
						//tüm başarımları sıfırlıyoruz
						if ($basarimSifirla=$conn->prepare("delete from basarimKontrol where googleID=?")){
							$basarimSifirla->bind_param("s", $googleID);
							$basarimSifirla->execute();
							$basarimSifirla->close();
						}
						
						//tüm mağaza bilgilerini önemli log olduğu için sıfırlamıyoruz
						//-
						
						//kullanılan kodları sıfırlıyoruz
						if ($kodSifirla=$conn->prepare("delete from kullanilanKodlar where googleID=?")){
							$kodSifirla->bind_param("s", $googleID);
							$kodSifirla->execute();
							$kodSifirla->close();
						}
						
						//oyuncu ayarlarını sıfırlıyoruz
						if ($ayarSifirla=$conn->prepare("delete from oAyarlari where googleID=?")){
							$ayarSifirla->bind_param("s", $googleID);
							$ayarSifirla->execute();
							$ayarSifirla->close();
						}
						
						
						$return="ok";
						
						
						echo($return);
					}
					else if ($durum=="boa")//başarımı tamamladığında jeton ödülü alma
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$basarimID=(isset($_GET['bid']) ? $_GET['bid'] : null);
						
						$return="";
						
						$odulSayisi=0;
						
						//verilecek ödül sayısını çekiyoruz
						$odulSayisiCek=$conn->prepare("select odul from basarimlar where id=? limit 1");
						$odulSayisiCek->bind_param("s", $basarimID);
						$odulSayisiCek->execute();
						$odulSayisiCek=$odulSayisiCek->get_result();
						if($_sc = mysqli_fetch_assoc($odulSayisiCek)){
							$odulSayisi=$_sc["odul"];
						}

						//başarımdan kazandığı jetonları veriyoruz
						if ($koduKullan=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+? where googleID=?")){
							$koduKullan->bind_param("ss", $odulSayisi, $googleID);
							$koduKullan->execute();
							$koduKullan->close();
						}
						
						//başarımın verildiğini başarım kontrol tablosunu düzenleyerek kaydediyoruz
						if ($odulAlindiKaydet=$conn->prepare("update basarimKontrol set odulAlindi=1 where basarimID=? and googleID=?")){
							$odulAlindiKaydet->bind_param("ss", $basarimID, $googleID);
							$odulAlindiKaydet->execute();
							$odulAlindiKaydet->close();
						}
						
						$return="ok";
						
						
						echo($return);
					}
					else if ($durum=="bkc")//başarım kontrol çek
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						if ($googleID!=null){
							$return="";
							
							//burada başarımlar tablosu ile basarimKontrol tablosunun eşit sayıda kayıta sahip olup olmadığını kontrol ediyoruz
							//eğer aynı değilse her başarımlar kaydı için basarimKontrol tablosuna bir kayıt ekliyoruz (odulAlindi için, ödülün alınıp alınmadığını kontrol edebilmek için)
							
							$kayitSayisi=0;
							$basarimIDleri=array();
							$basarimCek=$conn->prepare("select id from basarimlar order by id");
							$basarimCek->execute();
							$basarimCek=$basarimCek->get_result();
							while($_sic = mysqli_fetch_assoc($basarimCek)){
								array_push($basarimIDleri,$_sic["id"]);
								$kayitSayisi++;
							}
							
							$BKkayitSayisi=0;
							$basarimKontrolIDleri=array();
							$basarimCek1=$conn->prepare("select basarimID from basarimKontrol where googleID=?");
							$basarimCek1->bind_param("s" ,$googleID);
							$basarimCek1->execute();
							$basarimCek1=$basarimCek1->get_result();
							while($_sic = mysqli_fetch_assoc($basarimCek1)){
								array_push($basarimKontrolIDleri,$_sic["basarimID"]);
								$BKkayitSayisi++;
							}
							
							
							
							$eklenecekBasarimIDleri=array();
							for ($i = 0; $i < count($basarimIDleri); $i++) {
								if (!in_array($basarimIDleri[$i],$basarimKontrolIDleri)){
									array_push($eklenecekBasarimIDleri,$basarimIDleri[$i]);
								}
							}
							
							//eğer daha önce hiç basarimKontrol tablosuna kayıt eklenmediyse tüm başarımları ekliyoruz
							if ($BKkayitSayisi==0){
								for ($i = 0; $i < count($basarimIDleri); $i++) {
									if ($odulAlindiKaydet=$conn->prepare("insert into basarimKontrol(basarimID,odulAlindi,googleID) values(?,0,?)")){
										$odulAlindiKaydet->bind_param("ss", $basarimIDleri[$i], $googleID);
										$odulAlindiKaydet->execute();
										$odulAlindiKaydet->close();
									}
								}
							}
							else{
								//Eğer başarım kontrol tablosunda yoksa ekliyoruz ve odulAlindi değerini 0 yapıyoruz
								for ($i = 0; $i < count($eklenecekBasarimIDleri); $i++) {
									if ($odulAlindiKaydet=$conn->prepare("insert into basarimKontrol(basarimID,odulAlindi,googleID) values(?,0,?)")){
										$odulAlindiKaydet->bind_param("ss", $eklenecekBasarimIDleri[$i], $googleID);
										$odulAlindiKaydet->execute();
										$odulAlindiKaydet->close();
									}
								}
							}
							
							$basarimCek=$conn->prepare("select * from basarimKontrol where googleID=? order by id");
							$basarimCek->bind_param("s" ,$googleID);
							$basarimCek->execute();
							$basarimCek=$basarimCek->get_result();
							while($_sic = mysqli_fetch_assoc($basarimCek)){
								$return.=
								'{"id":"'.$_sic["id"].'",'.
								'"basarimID":"'.$_sic["basarimID"].'",'.
								'"odulAlindi":"'.($_sic["odulAlindi"]=="1"?"True":"False").'",'.
								'"googleID":"'.$_sic["googleID"].'"},';
							}
							if ($return!="") $return='['.substr($return, 0, -1).']';
							echo($return);
							
						}
					}
					else if ($durum=="jtk")//mağaza reklam için son alınan reklam jetonu tarihini kontrol et
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$return=$googleID;
						
						$simdikiTarih=date("Y-m-d H:i:s");
						
						$return="";
						$sonJetonTarihiCek=$conn->prepare("select sonJetonTarihi from jetonlar where googleID=? limit 1");
						$sonJetonTarihiCek->bind_param("s", $googleID);
						$sonJetonTarihiCek->execute();
						$sonJetonTarihiCek=$sonJetonTarihiCek->get_result();
						if($_sc = mysqli_fetch_assoc($sonJetonTarihiCek)){
							$sonJetonTarihi=$_sc["sonJetonTarihi"];
						}
						
						if ($sonJetonTarihi!=""){
							//eğer son jeton alım tarihinin üstünden 20 dk geçmişse tekrar reklam izleyebilir
							if (strtotime($simdikiTarih)>=strtotime($sonJetonTarihi."+ ".$reklamBeklemeSuresiDK." minute")){
								$return="ok";
							}
							else{
								$return=kacDkKaldi($sonJetonTarihi,$reklamBeklemeSuresiDK)." sonra";
							}
							
						}
						

						echo($return);
					}
					else if ($durum=="jta")//mağaza reklam izlendikten sonra jeton odulunu al
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$simdikiTarih=date("Y-m-d H:i:s");
						
						$return="";
						
						//jetonları veriyoruz
						if ($jetonVer=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+?,sonJetonTarihi=? where googleID=?")){
							$jetonVer->bind_param("sss", $magazaReklamVerilecekJeton, $simdikiTarih, $googleID);
							$jetonVer->execute();
							$jetonVer->close();
						}
						
						
						//magaza tablosuna jeton verildiğini kaydediyoruz
						if ($magazaIslemKaydet=$conn->prepare("insert into magaza(islem,miktar,durum,tarih,googleID) values('Mağaza Ücretsiz Jeton',?,1,?,?)")){
							$magazaIslemKaydet->bind_param("sss", $magazaReklamVerilecekJeton, $simdikiTarih, $googleID);
							$magazaIslemKaydet->execute();
							$magazaIslemKaydet->close();
						}
						
						$return="ok";
						
						
						echo($return);
					}
					else if ($durum=="jtd")//sandık reklam için son alınan reklam jetonu tarihini kontrol et
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						
						$return=$googleID;
						
						$simdikiTarih=date("Y-m-d H:i:s");
						
						$return="";
						$sonJetonTarihiCek=$conn->prepare("select gunlukJeton from jetonlar where googleID=? limit 1");
						$sonJetonTarihiCek->bind_param("s", $googleID);
						$sonJetonTarihiCek->execute();
						$sonJetonTarihiCek=$sonJetonTarihiCek->get_result();
						if($_sc = mysqli_fetch_assoc($sonJetonTarihiCek)){
							$gunlukJeton=$_sc["gunlukJeton"];
						}
						
						if ($gunlukJeton!=""){
							//eğer son sandık açılma tarihinin üstünden 24 saat geçmişse tekrar sandık açabilir
							if (strtotime($simdikiTarih)>=strtotime($gunlukJeton."+ ".$sandikBeklemeSuresiSA." hour")){
								$return="yes";
							}
							else{
								$return="no";
							}
						}
						
						echo($return);
					}
					else if ($durum=="jts")//sandık tekrar açılıp reklam izlendikten sonra jeton odulunu al
					{
						$googleID=(isset($_GET['_g']) ? $_GET['_g'] : null);
						$verilecekJeton=(isset($_GET['_j']) ? $_GET['_j'] : null);
						$videoIzlediMi=(isset($_GET['_t']) ? $_GET['_t'] : null);
						
						$simdikiTarih=date("Y-m-d H:i:s");
						
						$return="";
						
						if ($verilecekJeton>=1 && $verilecekJeton<=10){
						
							if ($videoIzlediMi=="e"){
								//jetonları veriyoruz
								if ($jetonVer=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+?,gunlukJeton=? where googleID=?")){
									$jetonVer->bind_param("sss", $verilecekJeton, $simdikiTarih, $googleID);
									$jetonVer->execute();
									$jetonVer->close();
								}
								
								
								//magaza tablosuna jeton verildiğini kaydediyoruz
								if ($magazaIslemKaydet=$conn->prepare("insert into magaza(islem,miktar,durum,tarih,googleID) values('Sandık Reklamlı Jeton',?,1,?,?)")){
									$magazaIslemKaydet->bind_param("sss", $verilecekJeton, $simdikiTarih, $googleID);
									$magazaIslemKaydet->execute();
									$magazaIslemKaydet->close();
								}
							}
							else if ($videoIzlediMi=="h"){
								//jetonları veriyoruz
								if ($jetonVer=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+?,gunlukJeton=? where googleID=?")){
									$jetonVer->bind_param("sss", $verilecekJeton, $simdikiTarih, $googleID);
									$jetonVer->execute();
									$jetonVer->close();
								}
								
								
								//magaza tablosuna jeton verildiğini kaydediyoruz
								if ($magazaIslemKaydet=$conn->prepare("insert into magaza(islem,miktar,durum,tarih,googleID) values('Sandık Günlük Jeton',?,1,?,?)")){
									$magazaIslemKaydet->bind_param("sss", $verilecekJeton, $simdikiTarih, $googleID);
									$magazaIslemKaydet->execute();
									$magazaIslemKaydet->close();
								}
							}
							
							$return="ok";
							
						}
						
						
						echo($return);
					}
					
					
				}
				else {
					header('Content-Type: application/x-www-form-urlencoded');
					
					
					$durum=(isset($_POST['_d']) ? $_POST['_d'] : null);
					
					
					//echo("input: ".file_get_contents('php://input'));
					
					if ($durum=="jto"){//jeton bilgilerini ekle
						
						$googleID=(isset($_POST['_p1']) ? $_POST['_p1'] : null);
						
						$bosTarih=date("1995-01-01 00:00:00");
						
						$jetonSayisi=0;
						
						$return="";
						
						if ($jetonEkle=$conn->prepare("insert into jetonlar(jetonSayisi,sonJetonTarihi,googleID,gunlukJeton) values(?,?,?,?)")){
							$jetonEkle->bind_param("ssss", $jetonSayisi, $bosTarih, $googleID, $bosTarih);
							$jetonEkle->execute();
							$jetonEkle->close();
							$return="ok";
						}
						

						echo($return);
						
					}
					else if ($durum=="jsa"){//jeton satın aldıktan sonra jeton yüklenmesi
						

						$googleID=(isset($_POST['_p0']) ? $_POST['_p0'] : null);
						$satinAlinanJetonSayisi=(isset($_POST['_p1']) ? $_POST['_p1'] : null);
						$donenJson=(isset($_POST['_p2']) ? $_POST['_p2'] : null);
						
						$simdikiTarih=date("Y-m-d H:i:s");
						
						$return="";
						
						if ($satinAlinanJetonSayisi==20 || $satinAlinanJetonSayisi==50 || $satinAlinanJetonSayisi==120 || 
							$satinAlinanJetonSayisi==320 || $satinAlinanJetonSayisi==750){
						
							//satın aldığı jetonları yüklüyoruz
							if ($jetonYukle=$conn->prepare("update jetonlar set jetonSayisi=jetonSayisi+? where googleID=?")){
								$jetonYukle->bind_param("ss", $satinAlinanJetonSayisi, $googleID);
								$jetonYukle->execute();
								$jetonYukle->close();
							}
							
							
							//magaza tablosuna jeton satın alındığını kaydediyoruz
							if ($magazaSatinAlimKaydet=$conn->prepare("insert into magaza(islem,donenJson,miktar,durum,tarih,googleID) values('Mağaza Jeton Satın Alma',?,?,1,?,?)")){
								$magazaSatinAlimKaydet->bind_param("ssss", $donenJson, $satinAlinanJetonSayisi, $simdikiTarih, $googleID);
								$magazaSatinAlimKaydet->execute();
								$magazaSatinAlimKaydet->close();
							}
							
							$return="ok";
						
						}
						
						
						echo($return);
						
					}
					else if ($durum=="ayk"){//oyuncu ayarlarını düzenle
						
						$googleID=(isset($_POST['_p0']) ? $_POST['_p0'] : null);
						$sesler=(isset($_POST['_p1']) ? $_POST['_p1'] : null);
						$muzik=(isset($_POST['_p2']) ? $_POST['_p2'] : null);
						$titresim=(isset($_POST['_p3']) ? $_POST['_p3'] : null);
						$yanlisKelimeSil=(isset($_POST['_p4']) ? $_POST['_p4'] : null);
						
						if ($googleID!=null && $sesler!=null && $muzik!=null && $titresim!=null && $yanlisKelimeSil!=null){
						
							$return="";
						
							$sesler=($sesler=="True"?"1":"0");
							$muzik=($muzik=="True"?"1":"0");
							$titresim=($titresim=="True"?"1":"0");
							$yanlisKelimeSil=($yanlisKelimeSil=="True"?"1":"0");
						
							$ayarVar=false;
							$ayarTanimlimi=$conn->prepare("select sesler from oAyarlari where googleID=? limit 1");
							$ayarTanimlimi->bind_param("s", $googleID);
							$ayarTanimlimi->execute();
							$ayarTanimlimi=$ayarTanimlimi->get_result();
							if($_sc = mysqli_fetch_assoc($ayarTanimlimi)){
								$ayarVar=true;
							}
							
							//bu oyuncu için ayarlar daha önce eklenmemişse ekliyoruz
							if (!$ayarVar){
								if ($ayarEkle=$conn->prepare("insert into oAyarlari(googleID,sesler,muzik,titresim,yanlisKelimeSil) values(?,?,?,?,?)")){
									$ayarEkle->bind_param("sssss", $googleID, $sesler, $muzik, $titresim, $yanlisKelimeSil);
									$ayarEkle->execute();
									$ayarEkle->close();
								}
							}
							
							//ayarları güncelliyoruz
							if ($ayarGuncelle=$conn->prepare("update oAyarlari set sesler=?,muzik=?,titresim=?,yanlisKelimeSil=? where googleID=?")){
								$ayarGuncelle->bind_param("sssss", $sesler, $muzik, $titresim, $yanlisKelimeSil, $googleID);
								$ayarGuncelle->execute();
								$ayarGuncelle->close();
								$return="ok";
							}
							
							echo($return);
						}

						
					}
					else if ($durum=="sie" || $durum=="sig" || $durum=="sig2" || 
					$durum=="sig3" || $durum=="sig4" || $durum=="sig5" || $durum=="sig6") { // soru istatistik ekle veya soru istatistik güncelle ise
						
					
						$soruID=(isset($_POST['_p0']) ? $_POST['_p0'] : null);
						$bolumGecildi=(isset($_POST['_p1']) ? $_POST['_p1'] : null);
						$hangiHarflerAcildi=(isset($_POST['_p2']) ? $_POST['_p2'] : null);//boş olabilir
						$toplamSure=(isset($_POST['_p3']) ? $_POST['_p3'] : null);
						$yanlisDenemeSayisi=(isset($_POST['_p4']) ? $_POST['_p4'] : null);
						$gorselSahneSayisi=(isset($_POST['_p6']) ? $_POST['_p6'] : null);
						$suAnkiGorselSahneSuresi=(isset($_POST['_p7']) ? $_POST['_p7'] : null);
						$uzaklastirmaSayisi=(isset($_POST['_p8']) ? $_POST['_p8'] : null);
						$harfAlmaSayisi=(isset($_POST['_p9']) ? $_POST['_p9'] : null);
						$kelimeAcildi=(isset($_POST['_p10']) ? $_POST['_p10'] : null);
						$toplamPuan=(isset($_POST['_p11']) ? $_POST['_p11'] : null);
						$acilanHarfler=(isset($_POST['_p12']) ? $_POST['_p12'] : null);//boş olabilir
						$googleID=(isset($_POST['_p13']) ? $_POST['_p13'] : null);

						$bolumGecildi=($bolumGecildi=="True"?"1":"0");
						$kelimeAcildi=($kelimeAcildi=="True"?"1":"0");
						
						
						$vt_bolumGecildi="";
						
						if ($googleID!=null && $soruID!=null){
							$soruIstatistikCek=$conn->prepare("select bolumGecildi from soruIstatistik where soruID=? and googleID=? limit 1");
							$soruIstatistikCek->bind_param("ss" ,$googleID,$googleID);
							$soruIstatistikCek->execute();
							$soruIstatistikCek=$soruIstatistikCek->get_result();
							if($_sic = mysqli_fetch_assoc($soruIstatistikCek)){
								$vt_bolumGecildi=$_sic["bolumGecildi"];
							}
						}
						
						//bölüm geçilmediğini bir de sunucu tarafında kontrol ediyoruz
						if ($vt_bolumGecildi=="0" || $vt_bolumGecildi==""){
							
							//150. sorudan sonra görsel geri saymayacağı için süreyi güncellemeye gerek yok, o yüzden $q aşağıda kullanıldı
							$q=intval($soruID)<=150;
							
							if ($durum=="sie")//soru istatistik ekle
							{
								$return="";
								
								if ($soruIstatistikEkle=$conn->prepare("insert into soruIstatistik(soruID,bolumGecildi,hangiHarflerAcildi,toplamSure,yanlisDenemeSayisi,gorselSahneSayisi,suAnkiGorselSahneSuresi,uzaklastirmaSayisi,harfAlmaSayisi,kelimeAcildi,toplamPuan,acilanHarfler,googleID) values(?,?,?,?,?,?,?,?,?,?,?,?,?)")){
									$soruIstatistikEkle->bind_param("sssssssssssss", $soruID, $bolumGecildi, $hangiHarflerAcildi, $toplamSure, $yanlisDenemeSayisi, $gorselSahneSayisi, $suAnkiGorselSahneSuresi, $uzaklastirmaSayisi, $harfAlmaSayisi, $kelimeAcildi, $toplamPuan, $acilanHarfler, $googleID);
									$soruIstatistikEkle->execute();
									$soruIstatistikEkle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig")//soru istatistik güncelle - hepsi
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set bolumGecildi=?,hangiHarflerAcildi=?,toplamSure=?,yanlisDenemeSayisi=?,gorselSahneSayisi=?,".($q?"suAnkiGorselSahneSuresi=?,":"")."uzaklastirmaSayisi=?,harfAlmaSayisi=?,kelimeAcildi=?,toplamPuan=?,acilanHarfler=? where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("sssssssssssss", $bolumGecildi, $hangiHarflerAcildi, $toplamSure, $yanlisDenemeSayisi, $gorselSahneSayisi, $suAnkiGorselSahneSuresi, $uzaklastirmaSayisi, $harfAlmaSayisi, $kelimeAcildi, $toplamPuan, $acilanHarfler, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("ssssssssssss", $bolumGecildi, $hangiHarflerAcildi, $toplamSure, $yanlisDenemeSayisi, $gorselSahneSayisi, $uzaklastirmaSayisi, $harfAlmaSayisi, $kelimeAcildi, $toplamPuan, $acilanHarfler, $soruID, $googleID);
									
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig2")//soru istatistik güncelle - geriCik
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set toplamSure=?".($q?",suAnkiGorselSahneSuresi=?":"")." where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("ssss", $toplamSure, $suAnkiGorselSahneSuresi, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("sss", $toplamSure, $soruID, $googleID);
										
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig3")//soru istatistik güncelle - sahneBitti
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set toplamSure=?,gorselSahneSayisi=?".($q?",suAnkiGorselSahneSuresi=?":"")." where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("sssss", $toplamSure, $gorselSahneSayisi, $suAnkiGorselSahneSuresi, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("ssss", $toplamSure, $gorselSahneSayisi, $soruID, $googleID);
									
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig4")//soru istatistik güncelle - gorselUzaklastirildi
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set toplamSure=?,gorselSahneSayisi=?,".($q?"suAnkiGorselSahneSuresi=?,":"")."uzaklastirmaSayisi=? where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("ssssss", $toplamSure, $gorselSahneSayisi, $suAnkiGorselSahneSuresi, $uzaklastirmaSayisi, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("sssss", $toplamSure, $gorselSahneSayisi, $uzaklastirmaSayisi, $soruID, $googleID);
										
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig5")//soru istatistik güncelle - harfAlindi
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set hangiHarflerAcildi=?,toplamSure=?,".($q?"suAnkiGorselSahneSuresi=?,":"")."harfAlmaSayisi=?,acilanHarfler=? where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("sssssss", $hangiHarflerAcildi, $toplamSure, $suAnkiGorselSahneSuresi, $harfAlmaSayisi, $acilanHarfler, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("ssssss", $hangiHarflerAcildi, $toplamSure, $harfAlmaSayisi, $acilanHarfler, $soruID, $googleID);
										
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							else if ($durum=="sig6")//soru istatistik güncelle - yanlisCevapVerildi
							{
								$return="";
								
								if ($soruIstatistikGuncelle=$conn->prepare("update soruIstatistik set toplamSure=?,yanlisDenemeSayisi=?".($q?",suAnkiGorselSahneSuresi=?":"")." where soruID=? and googleID=?")){
									if ($q)
										$soruIstatistikGuncelle->bind_param("sssss", $toplamSure, $yanlisDenemeSayisi, $suAnkiGorselSahneSuresi, $soruID, $googleID);
									else
										$soruIstatistikGuncelle->bind_param("ssss", $toplamSure, $yanlisDenemeSayisi, $soruID, $googleID);
										
									$soruIstatistikGuncelle->execute();
									$soruIstatistikGuncelle->close();
									$return="ok";
								}
								
								echo($return);
							}
							
						}
						else echo("ok");//eğer bölüm geçilmişse veri eklenmiş gibi davranıyoruz 
					}
					
				}
				}
				else{//version aynı değilse
					echo("verup");
				}
			}
			else{//uygulama aktif değilse
				echo("bakim");
			}
		}
	}
}





function get_client_ip() {
    $ipaddress = '';
    if (isset($_SERVER['HTTP_CLIENT_IP']))
        $ipaddress = $_SERVER['HTTP_CLIENT_IP'];
    else if(isset($_SERVER['HTTP_X_FORWARDED_FOR']))
        $ipaddress = $_SERVER['HTTP_X_FORWARDED_FOR'];
    else if(isset($_SERVER['HTTP_X_FORWARDED']))
        $ipaddress = $_SERVER['HTTP_X_FORWARDED'];
    else if(isset($_SERVER['HTTP_FORWARDED_FOR']))
        $ipaddress = $_SERVER['HTTP_FORWARDED_FOR'];
    else if(isset($_SERVER['HTTP_FORWARDED']))
        $ipaddress = $_SERVER['HTTP_FORWARDED'];
    else if(isset($_SERVER['REMOTE_ADDR']))
        $ipaddress = $_SERVER['REMOTE_ADDR'];
    else
        $ipaddress = 'UNKNOWN';
    return $ipaddress;
}

function kacDkKaldi($time,$dk) {
	$saniye=strtotime($time);
	$kalanSaniye=time()-$saniye;
    //$kalanSaniye=($kalanSaniye<1)?1:$kalanSaniye;//bu satır kullanılmazsa daha ileri tarihleri de gösterir (örneğin: 20 dk dan fazla zaman girilir ise 300 dk kaldı tarzında sonuç verir)
	$_dk = floor($kalanSaniye / 60);
	$kalanZaman=$dk-$_dk;
	
	if ($kalanZaman==1){
		$_sn = floor($kalanSaniye / 1);
		$kalanZaman=($dk*60)-$_sn;
		$kalanZaman=$kalanZaman." saniye";
	}
	else $kalanZaman=$kalanZaman." dakika";
	
	return $kalanZaman;
}



?>