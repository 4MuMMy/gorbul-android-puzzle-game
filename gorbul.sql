-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Anamakine: 127.0.0.1:3307
-- Üretim Zamanı: 14 May 2024, 05:03:29
-- Sunucu sürümü: 11.3.2-MariaDB
-- PHP Sürümü: 8.2.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Veritabanı: `gorbul`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `ayarlar`
--

DROP TABLE IF EXISTS `ayarlar`;
CREATE TABLE IF NOT EXISTS `ayarlar` (
  `aktif` tinyint(1) NOT NULL,
  `duyuru` varchar(600) DEFAULT NULL,
  `version` varchar(50) NOT NULL,
  PRIMARY KEY (`aktif`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Tablo döküm verisi `ayarlar`
--

INSERT INTO `ayarlar` (`aktif`, `duyuru`, `version`) VALUES
(1, '', '1.0.64');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `basarimkontrol`
--

DROP TABLE IF EXISTS `basarimkontrol`;
CREATE TABLE IF NOT EXISTS `basarimkontrol` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `basarimID` int(11) NOT NULL,
  `odulAlindi` tinyint(1) NOT NULL,
  `googleID` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1590 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `basarimlar`
--

DROP TABLE IF EXISTS `basarimlar`;
CREATE TABLE IF NOT EXISTS `basarimlar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `baslik` varchar(100) NOT NULL,
  `aciklama` varchar(200) NOT NULL,
  `odul` int(11) NOT NULL,
  `hedefDeger` int(11) NOT NULL,
  `achi_id` varchar(18) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Tablo döküm verisi `basarimlar`
--

INSERT INTO `basarimlar` (`id`, `baslik`, `aciklama`, `odul`, `hedefDeger`, `achi_id`) VALUES
(1, 'En zor adım, ilk adım!', '1 bulmaca çözün.', 5, 1, 'x'),
(2, 'Üçüncü bulmacanı da çözdün!', '3 bulmaca çözün.', 5, 3, 'x'),
(3, 'Onda on!', '10 bulmaca çözün.', 10, 10, 'x'),
(4, 'Bölüm 1: Hayvanlar', 'Bölüm 1: Hayvanlar bölümünü tamamlayın.', 10, 40, 'x'),
(5, 'Ellisi de ellisii!', '50 bulmaca çözün.', 15, 50, 'x'),
(6, 'Bölüm 2: Bitkiler', 'Bölüm 2: Bitkiler bölümünü tamamlayın.', 10, 80, 'x'),
(7, 'Yüz soru, yüz cevap!', '100 bulmaca çözün.', 20, 100, 'x'),
(8, 'Bölüm 3: Meyveler', 'Bölüm 3: Meyveler bölümünü tamamlayın.', 20, 120, 'x');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `bolumler`
--

DROP TABLE IF EXISTS `bolumler`;
CREATE TABLE IF NOT EXISTS `bolumler` (
  `id` int(11) NOT NULL,
  `bolumAdi` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Tablo döküm verisi `bolumler`
--

INSERT INTO `bolumler` (`id`, `bolumAdi`) VALUES
(1, 'Hayvanlar'),
(2, 'Bitkiler'),
(3, 'Meyveler');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `jetonlar`
--

DROP TABLE IF EXISTS `jetonlar`;
CREATE TABLE IF NOT EXISTS `jetonlar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `jetonSayisi` int(11) NOT NULL,
  `sonJetonTarihi` datetime DEFAULT NULL,
  `googleID` varchar(300) NOT NULL,
  `gunlukJeton` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=283 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `kodlar`
--

DROP TABLE IF EXISTS `kodlar`;
CREATE TABLE IF NOT EXISTS `kodlar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `kod` varchar(8) NOT NULL,
  `odulJeton` int(11) NOT NULL,
  `bitisTarihi` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `kod` (`kod`),
  KEY `id_2` (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `kullanilankodlar`
--

DROP TABLE IF EXISTS `kullanilankodlar`;
CREATE TABLE IF NOT EXISTS `kullanilankodlar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `googleID` varchar(300) NOT NULL,
  `kullanilanKod` varchar(8) NOT NULL,
  `kullanilanTarih` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `log`
--

DROP TABLE IF EXISTS `log`;
CREATE TABLE IF NOT EXISTS `log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `txt` text DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `magaza`
--

DROP TABLE IF EXISTS `magaza`;
CREATE TABLE IF NOT EXISTS `magaza` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `islem` varchar(50) NOT NULL,
  `donenJson` text DEFAULT NULL,
  `miktar` int(11) NOT NULL,
  `durum` int(1) NOT NULL,
  `tarih` datetime NOT NULL,
  `googleID` varchar(300) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1289 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `oayarlari`
--

DROP TABLE IF EXISTS `oayarlari`;
CREATE TABLE IF NOT EXISTS `oayarlari` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `googleID` varchar(300) NOT NULL,
  `sesler` tinyint(1) NOT NULL,
  `muzik` tinyint(1) NOT NULL,
  `titresim` tinyint(1) NOT NULL,
  `yanlisKelimeSil` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=136 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `skortablolari`
--

DROP TABLE IF EXISTS `skortablolari`;
CREATE TABLE IF NOT EXISTS `skortablolari` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ilk` int(11) NOT NULL,
  `son` int(11) NOT NULL,
  `lead_id` varchar(18) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Tablo döküm verisi `skortablolari`
--

INSERT INTO `skortablolari` (`id`, `ilk`, `son`, `lead_id`) VALUES
(1, 1, 40, 'x'),
(2, 41, 200, 'x'),
(3, 201, 400, 'x'),
(4, 401, 600, 'x'),
(5, 601, 800, 'x'),
(6, 801, 1000, 'x');

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `soruistatistik`
--

DROP TABLE IF EXISTS `soruistatistik`;
CREATE TABLE IF NOT EXISTS `soruistatistik` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `soruID` int(11) NOT NULL,
  `bolumGecildi` tinyint(1) NOT NULL,
  `hangiHarflerAcildi` varchar(110) DEFAULT NULL,
  `toplamSure` int(11) NOT NULL,
  `yanlisDenemeSayisi` int(11) NOT NULL,
  `gorselSahneSayisi` int(11) NOT NULL,
  `suAnkiGorselSahneSuresi` int(11) NOT NULL,
  `uzaklastirmaSayisi` int(11) NOT NULL,
  `harfAlmaSayisi` int(11) NOT NULL,
  `kelimeAcildi` tinyint(1) NOT NULL,
  `toplamPuan` int(11) NOT NULL,
  `acilanHarfler` varchar(110) DEFAULT NULL,
  `googleID` varchar(300) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3479 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `sorular`
--

DROP TABLE IF EXISTS `sorular`;
CREATE TABLE IF NOT EXISTS `sorular` (
  `id` int(11) NOT NULL,
  `soru` varchar(200) NOT NULL,
  `cevap` varchar(30) NOT NULL,
  `gorselSayisi` int(11) NOT NULL DEFAULT 4,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Tablo döküm verisi `sorular`
--

INSERT INTO `sorular` (`id`, `soru`, `cevap`, `gorselSayisi`) VALUES
(1, 'Yukarıda görmüş olduğunuz uzuv hangi hayvana aittir?', 'Fil', 4),
(2, 'Yukarıda görülen hangi hayvanın kulaklarıdır?', 'Tay', 4),
(3, 'Yukarıda gördüğünüz hangi hayvanın kuyruğudur?', 'Kuş', 4),
(4, 'Yukarıdaki burun hangi hayvana aittir? ', 'Ayı', 4),
(5, 'Yukarıdaki hayvan nedir?', 'Arı', 4),
(6, 'Yukarıdaki perdeli ayaklara sahip hayvan nedir? ', 'Kaz', 4),
(7, 'Yukarıdaki hayvan nedir?', 'Koç', 4),
(8, 'Denizde daha rahat hareket edebilen yukarıdaki hayvanın adı nedir?', 'Fok', 4),
(9, 'Bu kuyruk hangi hayvana aittir?', 'Kedi', 4),
(10, 'Bu ayaklar hangi hayvanındır?', 'İnek', 4),
(11, 'Yukarıda görülen hangi hayvanın bir kısmıdır?', 'Deve', 4),
(12, 'Yukarıda gördüğünüz hayvan nedir?', 'Pars', 4),
(13, 'Yukarıdaki nedir?', 'Keçi', 4),
(14, 'Yukarıdaki nedir?', 'Saka', 4),
(15, 'Yukarıdaki nedir?', 'Fare', 4),
(16, 'Yukarıdaki nedir?', 'Kuzu', 4),
(17, 'Yukarıdaki nedir?', 'Kurt', 4),
(18, 'Yukarıdaki nedir?', 'Eşek', 4),
(19, 'Yukarıdaki nedir?', 'Lama', 4),
(20, 'Yukarıdaki nedir?', 'Çita', 4),
(21, 'Görseldeki nedir?', 'Kuğu', 4),
(76, 'Yukarıdaki nedir?', 'Gelincik', 4),
(22, 'Görseldeki nedir?', 'Balık', 4),
(23, 'Görseldeki nedir?', 'Panda', 4),
(24, 'Görseldeki nedir?', 'Rakun', 4),
(25, 'Görseldeki nedir?', 'Zebra', 4),
(26, 'Üstteki nedir?', 'Horoz', 4),
(27, 'Üstteki nedir?', 'Geyik', 4),
(28, 'Üstteki nedir?', 'Vaşak', 4),
(29, 'Üstteki nedir?', 'Goril', 4),
(30, 'Üstteki nedir?', 'Lemur', 4),
(31, 'Üstteki nedir?', 'Domuz', 4),
(32, 'Üstteki nedir?', 'Yılan', 4),
(33, 'Yukarıda görmüş olduğunuz nedir?', 'Doğan', 4),
(34, 'Yukarıda görmüş olduğunuz nedir?', 'Çakal', 4),
(35, 'Yukarıda görmüş olduğunuz nedir?', 'Hindi', 4),
(36, 'Yukarıda görmüş olduğunuz nedir?', 'Aslan', 4),
(37, 'Yukarıda görmüş olduğunuz nedir?', 'Babun', 4),
(38, 'Yukarıda görmüş olduğunuz nedir?', 'Tapir', 4),
(39, 'Yukarıda görmüş olduğunuz nedir?', 'Tilki', 4),
(40, 'Yukarıda görmüş olduğunuz nedir?', 'Yunus', 4),
(41, 'Yukarıda görmüş olduğunuz nedir?', 'Çam', 4),
(75, 'Üstteki nedir?', 'Sarmaşık', 4),
(74, 'Üstteki nedir?', 'Balkabağı', 4),
(73, 'Üstteki nedir?', 'Ayçiçeği', 4),
(72, 'Üstteki nedir?', 'Zencefil', 4),
(71, 'Üstteki nedir?', 'Sarımsak', 4),
(70, 'Görseldeki nedir?', 'Greyfurt', 4),
(69, 'Görseldeki nedir?', 'Maydanoz', 4),
(68, 'Görseldeki nedir?', 'Patlıcan', 4),
(67, 'Görseldeki nedir?', 'Kuşburnu', 4),
(66, 'Görseldeki nedir?', 'Papatya', 4),
(65, 'Yukarıda görmüş olduğunuz nedir?', 'Lavanta', 4),
(64, 'Yukarıda görmüş olduğunuz nedir?', 'Palmiye', 4),
(63, 'Yukarıda görmüş olduğunuz nedir?', 'Ihlamur', 4),
(62, 'Yukarıda görmüş olduğunuz nedir?', 'Erguvan', 4),
(61, 'Yukarıda görmüş olduğunuz nedir?', 'Patates', 4),
(60, 'Yukarıdaki nedir?', 'Kestane', 4),
(59, 'Yukarıdaki nedir?', 'Avokado', 4),
(58, 'Yukarıdaki nedir?', 'Bezelye', 4),
(57, 'Yukarıdaki nedir?', 'Ahududu', 4),
(56, 'Yukarıdaki nedir?', 'Karpuz', 4),
(55, 'Üstteki nedir?', 'Orkide', 4),
(54, 'Üstteki nedir?', 'Ananas', 4),
(53, 'Üstteki nedir?', 'Kaktüs', 4),
(52, 'Üstteki nedir?', 'Yosun', 4),
(51, 'Üstteki nedir?', 'Bambu', 4),
(50, 'Görseldeki nedir?', 'Pamuk', 4),
(49, 'Görseldeki nedir?', 'Havuç', 4),
(48, 'Görseldeki nedir?', 'Mısır', 4),
(47, 'Görseldeki nedir?', 'İncir', 4),
(46, 'Görseldeki nedir?', 'Nane', 4),
(45, 'Görseldeki nedir?', 'Arpa', 4),
(44, 'Görseldeki nedir?', 'Asma', 4),
(43, 'Yukarıda görmüş olduğunuz nedir?', 'Gül', 4),
(42, 'Yukarıda görmüş olduğunuz nedir?', 'Çim', 4),
(78, 'Yukarıdaki nedir?', 'Kardelen', 4),
(77, 'Yukarıdaki nedir?', 'Hanımeli', 4),
(79, 'Yukarıdaki nedir?', 'Sardunya', 4),
(80, 'Yukarıdaki nedir?', 'Böğürtlen', 4),
(81, 'Yukarıda görmüş olduğunuz şey nedir?', 'Dut', 4),
(82, 'Yukarıda görmüş olduğunuz şey nedir?', 'Nar', 4),
(83, 'Yukarıda görmüş olduğunuz şey nedir?', 'Muz', 4),
(84, 'Yukarıda görmüş olduğunuz şey nedir?', 'Ayva', 4),
(85, 'Üstteki nedir?', 'Erik', 4),
(86, 'Üstteki nedir?', 'Elma', 4),
(87, 'Üstteki nedir?', 'Kivi', 4),
(88, 'Üstteki nedir?', 'Liçi', 4),
(89, 'Görseldeki meyvenin adı nedir?', 'Armut', 4),
(90, 'Görseldeki meyvenin adı nedir?', 'Çilek', 4),
(91, 'Görseldeki meyvenin adı nedir?', 'Kiraz', 4),
(92, 'Görseldeki meyvenin adı nedir?', 'Hurma', 4),
(93, 'Yukarıda görmüş olduğunuz nedir?', 'Badem', 4),
(94, 'Yukarıda görmüş olduğunuz nedir?', 'Kavun', 4),
(95, 'Yukarıda görmüş olduğunuz nedir?', 'Limon', 4),
(96, 'Görseldeki nedir?', 'Mango', 4),
(97, 'Görseldeki nedir?', 'Kabak', 4),
(98, 'Görseldeki nedir?', 'Kamkat', 4),
(99, 'Görseldeki nedir?', 'Kayısı', 4),
(100, 'Görseldeki nedir?', 'Muşmula', 4),
(101, 'Görseldeki meyvenin adı nedir?', 'Şeftali', 4),
(102, 'Görseldeki meyvenin adı nedir?', 'Kızılcık', 4),
(103, 'Görseldeki meyvenin adı nedir?', 'Nektarin', 4),
(104, 'Görseldeki meyvenin adı nedir?', 'Çerimoya', 4),
(105, 'Görseldeki meyvenin adı nedir?', 'Bergamot', 4),
(106, 'Görseldeki meyve nedir?', 'Kuşburnu', 4),
(107, 'Görseldeki meyve nedir?', 'Portakal', 4),
(108, 'Görseldeki meyve nedir?', 'Karambola', 4),
(109, 'Görseldeki meyve nedir?', 'Mandalina', 4),
(110, 'Görseldeki meyve nedir?', 'Kuş Kirazı', 4),
(111, 'Yukarıda görmüş olduğunuz meyve nedir?', 'Dağ Çileği', 4),
(112, 'Yukarıda görmüş olduğunuz meyve nedir?', 'Kurt Üzümü', 4),
(113, 'Yukarıda görmüş olduğunuz meyve nedir?', 'Yer Elması', 4),
(114, 'Yukarıda görmüş olduğunuz meyve nedir?', 'Kuş İğdesi', 4),
(115, 'Yukarıda görmüş olduğunuz meyve nedir?', 'Koca Yemiş', 4),
(116, 'Görseldeki meyveye ne ad verilir?', 'Acai Üzümü', 4),
(117, 'Görseldeki meyveye ne ad verilir?', 'Malta Eriği', 4),
(118, 'Görseldeki meyveye ne ad verilir?', 'Budanın Eli', 4),
(119, 'Görseldeki meyveye ne ad verilir?', 'Yaban Mersini', 4),
(120, 'Görseldeki meyveye ne ad verilir?', 'Boynuzlu Kavun', 4);

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `yasaklilar`
--

DROP TABLE IF EXISTS `yasaklilar`;
CREATE TABLE IF NOT EXISTS `yasaklilar` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `googleID` varchar(300) NOT NULL,
  `sebep` varchar(500) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
