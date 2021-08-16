using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Taş_Kağıt_Makas_Makine_Öğrenmesi___Online_Sıralama__
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        MySqlConnection baglantı = new MySqlConnection();
        MySqlCommand komut;
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1900;
        }
        List<int> puanlar = new List<int>();
        private void yükleniyor()
        {
            List<string> verilerim = new List<string>();
            puanlar.Clear();
            listBox1.Items.Clear();
            baglantı.Open();
            komut = new MySqlCommand();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From TKM");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                verilerim.Add(oku["kullaniciadi"].ToString() + "|" + oku["puan"].ToString());
                puanlar.Add(Convert.ToInt32(oku["puan"].ToString()));
            }
            baglantı.Close();
            Array veriler = puanlar.ToArray();
            Array.Sort(veriler);
            Array.Reverse(veriler);
            int sayac = 0;
            foreach(int veri in veriler)
            {
                puanlar[sayac] = veri;
                sayac++;
                for (int i=0;i<verilerim.Count;i++)
                {
                    if (verilerim[i].Split('|')[1]==veri.ToString())
                    {
                        listBox1.Items.Add(verilerim[i].Split('|')[0]);
                        verilerim.Remove(verilerim[i]);
                        break;
                    }
                }
            }
        }
        bool getirildimi = false;
        public void VeriKontrol()
        {
            try
            {
                baglantı.Open();
                komut = new MySqlCommand("SELECT * From TKM", baglantı);
                MySqlDataReader dr = komut.ExecuteReader();
            }
            catch
            {
                baglantı.Close();
                baglantı.Open();
                string tkmtablo = "CREATE TABLE TKM (kullaniciadi VARCHAR(50),sifre VARCHAR(50),tas1 VARCHAR(50),kagit1 VARCHAR(50),makas1 VARCHAR(50),tas2 VARCHAR(50),kagit2 VARCHAR(50),makas2 VARCHAR(50),tas3 VARCHAR(50),kagit3 VARCHAR(50),makas3 VARCHAR(50),tas4 VARCHAR(50),kagit4 VARCHAR(50),makas4 VARCHAR(50),tas5 VARCHAR(50),kagit5 VARCHAR(50),makas5 VARCHAR(50),puan VARCHAR(50))";
                string sunucu = "CREATE TABLE SUNUCU (kullaniciadi VARCHAR(50),sifre VARCHAR(50),oyuncu1 VARCHAR(50),oyuncu2 VARCHAR(50),oyun1 VARCHAR(50),oyun2 VARCHAR(50))";
                komut.CommandText = tkmtablo;
                komut.ExecuteReader();
                baglantı.Close();
                baglantı.Open();
                komut.CommandText = sunucu;
                komut.ExecuteReader();
                baglantı.Close();
                MessageBox.Show("Yeni bir sunucuya taşındık. Tüm veriler sıfırlandı yeniden hesap oluşturmanız gerekebilir.", "@kodzamani.tk");
            }
        }
        private void veritabanınıgetir()
        {
            string host = "";
            string port = "";
            string name = "";
            string password = "";
        dön:
            try
            {
                if (webBrowser1.Url == null)
                {
                    Thread.Sleep(2000); goto dön;
                }
                string veriOO = webBrowser1.Document.Body.InnerText;
                string[] verilerOOO = veriOO.Split('\n');
                for (int i = 0; i < verilerOOO.Length; i++)
                {
                    if (verilerOOO[i].Contains("kodzamani:") == true)
                    {
                        string[] islenmemişveriler = verilerOOO[i].Split(':');
                        host = islenmemişveriler[1];
                        port = islenmemişveriler[2];
                        name = islenmemişveriler[3];
                        password = islenmemişveriler[4];
                        break;
                    }
                }
            }
            catch { Thread.Sleep(2000); goto dön; }
            baglantı = new MySqlConnection($"datasource={host};port={port};Initial Catalog='{name}';username={name};password={password}");

            try
            {
                baglantı.Open();
                baglantı.Close();
                getirildimi = true;
                VeriKontrol();
            }
            catch
            {
                MessageBox.Show("Sunucuya bağlanılamadı. İletişim sayfasına yönlendiriliyorsunuz.", "@kodzamani.tk");
                Process.Start("https://kodzamani.weebly.com/iletisim.html");
                Application.Exit();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= 5)
            {
                if (textBox1.Text.Length >= 3)
                {
                    try
                    {
                        if (getirildimi == false)
                            veritabanınıgetir();
                        bool oluştur = true;
                        baglantı.Open();
                        komut = new MySqlCommand();
                        komut.Connection = baglantı;
                        komut.CommandText = ("Select * From TKM");
                        MySqlDataReader oku = komut.ExecuteReader();
                        while (oku.Read())
                        {
                            if (oku["kullaniciadi"].ToString() == textBox1.Text)
                                oluştur = false;
                        }
                        baglantı.Close();
                        if (oluştur == true)
                        {
                            baglantı.Open();
                            komut = new MySqlCommand("insert into TKM (kullaniciadi,sifre,tas1,kagit1,makas1,tas2,kagit2,makas2,tas3,kagit3,makas3,tas4,kagit4,makas4,tas5,kagit5,makas5,puan) values ('" + textBox1.Text + "','" + textBox2.Text + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "1" + "','" + "0" + "')", baglantı);
                            komut.ExecuteNonQuery();
                            baglantı.Close();
                            button3.PerformClick();
                            yükleniyor();
                        }
                        else
                            MessageBox.Show("Bu kullaniciadi zaten alınmış.", "@kodzamani.tk");
                    }
                    catch
                    {

                    }
                }
                else
                    MessageBox.Show("Kullaniciadi 3 haneden büyük 16 haneden küçük olmalı.", "@kodzamani.tk");
            }
            else
                MessageBox.Show("Şifre 5 haneden 32 haneden küçük olmalı.","@kodzamani.tk");
        }
        public string kullaniciadihesap = "";
        public string sifrehesap = "";
        public int[] taslar = new int[5];
        public int[] kagitlar = new int[5];
        public int[] makaslar = new int[5];
        public int puan = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (getirildimi == false)
                    veritabanınıgetir();
                bool kullaniciadi = false;
                bool şifre = false;
                baglantı.Open();
                komut = new MySqlCommand();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From TKM");
                MySqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["kullaniciadi"].ToString() == textBox1.Text)
                    {
                        kullaniciadi = true;
                        if (oku["sifre"].ToString() == textBox2.Text)
                        {
                            şifre = true;
                            kullaniciadihesap = oku["kullaniciadi"].ToString();
                            sifrehesap = oku["sifre"].ToString();
                            taslar[0] = Convert.ToInt32(oku["tas1"].ToString());
                            taslar[1] = Convert.ToInt32(oku["tas2"].ToString());
                            taslar[2] = Convert.ToInt32(oku["tas3"].ToString());
                            taslar[3] = Convert.ToInt32(oku["tas4"].ToString());
                            taslar[4] = Convert.ToInt32(oku["tas5"].ToString());
                            kagitlar[0] = Convert.ToInt32(oku["kagit1"].ToString());
                            kagitlar[1] = Convert.ToInt32(oku["kagit2"].ToString());
                            kagitlar[2] = Convert.ToInt32(oku["kagit3"].ToString());
                            kagitlar[3] = Convert.ToInt32(oku["kagit4"].ToString());
                            kagitlar[4] = Convert.ToInt32(oku["kagit5"].ToString());
                            makaslar[0] = Convert.ToInt32(oku["makas1"].ToString());
                            makaslar[1] = Convert.ToInt32(oku["makas2"].ToString());
                            makaslar[2] = Convert.ToInt32(oku["makas3"].ToString());
                            makaslar[3] = Convert.ToInt32(oku["makas4"].ToString());
                            makaslar[4] = Convert.ToInt32(oku["makas5"].ToString());
                            puan = Convert.ToInt32(oku["puan"].ToString());
                            label8.Text = "Puanınız : " + puan.ToString();
                            break;
                        }
                    }
                }
                baglantı.Close();
                if (kullaniciadi == true && şifre == true)
                {
                    Giriş.Visible = false;
                    button8.PerformClick();
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = true;
                    yükleniyor();
                }
                else if (kullaniciadi == true)
                    MessageBox.Show("Şifren yanlış lütfen tekrar dene.", "@kodzamani.tk");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Böyle bir hesap bulunamadı. Hesap oluşturmak istermisiniz ?", "@kodzamani.tk", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        button4.PerformClick();
                    }
                }
            }
            catch { }

        }
        Thread th;
        private void timer1_Tick(object sender, EventArgs e)
        {
            th = new Thread(animasyon);th.Start();
        }
        private void animasyon()
        {
            pictureBox4.Image = pictureBox1.Image;
            Thread.Sleep(600);
            pictureBox4.Image = pictureBox2.Image;
            Thread.Sleep(600);
            pictureBox4.Image = pictureBox3.Image;
            Thread.Sleep(600);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        Random rastgele = new Random();
        int berabersayac = 1;
        private void oyunuoyna(PictureBox picture)
        {
            timer1.Enabled = false;
            string veriseti = "";
            for (int i = 0; i < taslar[oyunsayac]; i++)
                veriseti += "k";
            for (int i = 0; i < kagitlar[oyunsayac]; i++)
                veriseti += "m";
            for (int i = 0; i < makaslar[oyunsayac]; i++)
                veriseti += "t";
            int hesapla = veriseti.Length;
            string bilgisayar = veriseti[rastgele.Next(hesapla)].ToString();
            if (bilgisayar == "t")
                pictureBox4.Image = pictureBox1.Image;
            if (bilgisayar == "k")
                pictureBox4.Image = pictureBox2.Image;
            if (bilgisayar == "m")
                pictureBox4.Image = pictureBox3.Image;
            string kullanıcı = "";
            if (picture.Name == "pictureBox1")
            {
                kullanıcı = "t";
                taslar[oyunsayac]++;
            }
            if (picture.Name == "pictureBox2")
            {
                kullanıcı = "k";
                kagitlar[oyunsayac]++;
            }
            if (picture.Name == "pictureBox3")
            {
                kullanıcı = "m";
                makaslar[oyunsayac]++;
            }
            if (kullanıcı != bilgisayar)
                oyunsayac++;
            if (kullanıcı == bilgisayar)
                berabersayac++;
            else if (kullanıcı == "t" && bilgisayar == "k")
            {
                puan -= 2;
                label6.Text = Convert.ToString(Convert.ToInt32(label6.Text) + 1);
            }
            else if (kullanıcı == "t" && bilgisayar == "m")
            {
                puan += 2;
                label5.Text = Convert.ToString(Convert.ToInt32(label5.Text) + 1);
            }
            else if (kullanıcı == "k" && bilgisayar == "t")
            {
                puan += 2;
                label5.Text = Convert.ToString(Convert.ToInt32(label5.Text) + 1);
            }
            else if (kullanıcı == "k" && bilgisayar == "m")
            {
                puan -= 2;
                label6.Text = Convert.ToString(Convert.ToInt32(label6.Text) + 1);
            }
            else if (kullanıcı == "m" && bilgisayar == "t")
            {
                puan -= 2;
                label6.Text = Convert.ToString(Convert.ToInt32(label6.Text) + 1);
            }
            else if (kullanıcı == "m" && bilgisayar == "k")
            {
                puan += 2;
                label5.Text = Convert.ToString(Convert.ToInt32(label5.Text) + 1);
            }
            if (label5.Text == "3")
            {
                puan += berabersayac;
                MessageBox.Show("Tebrikler oyunu " + label5.Text + "-" + label6.Text + " kazandınız.","@kodzamani.tk");
                label5.Text = "0";
                label6.Text = "0";
                oyunsayac = 0;
                berabersayac = 1;
                verilerigüncelle();
            }
            if (label6.Text == "3")
            {
                puan -= berabersayac;
                MessageBox.Show("Maalesef oyunu " + label6.Text + "-" + label5.Text + " kaybettiniz.", "@kodzamani.tk");
                label6.Text = "0";
                label5.Text = "0";
                oyunsayac = 0;
                berabersayac = 1;
                verilerigüncelle();
            }
            label8.Text = "Puanınız : "+ puan.ToString();
            timer1.Enabled = true;
        }
        private void verilerigüncelle()
        {
            baglantı.Open();
            komut = new MySqlCommand("delete from TKM where kullaniciadi = '" + kullaniciadihesap + "'", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            baglantı.Open();
            komut = new MySqlCommand("insert into TKM (kullaniciadi,sifre,tas1,kagit1,makas1,tas2,kagit2,makas2,tas3,kagit3,makas3,tas4,kagit4,makas4,tas5,kagit5,makas5,puan) values ('" + kullaniciadihesap + "','" + sifrehesap + "','" + taslar[0] + "','" + kagitlar[0] + "','" + makaslar[0] + "','" +taslar[1] + "','" + kagitlar[1] + "','" +makaslar[1]+ "','" + taslar[2] + "','" +kagitlar[2] + "','" + makaslar[2]+ "','" + taslar[3] + "','" + kagitlar[3] + "','" +makaslar[3] + "','" + taslar[4] + "','" +kagitlar[4] + "','" + makaslar[4]+ "','" +puan + "')", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            yükleniyor();
        }
        int oyunsayac = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            oyunuoyna(pictureBox1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            oyunuoyna(pictureBox2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            oyunuoyna(pictureBox3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Haydaa elime niye vuruyorsun ayıp değil mi :)", "@kodzamani.tk");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i=0;i<listBox1.Items.Count;i++)
            {
                if (listBox1.Items[i].ToString()==textBox1.Text)
                {
                    listBox1.SelectedIndex = i;
                    break;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Sıralama : "+(listBox1.SelectedIndex+1)+". " + listBox1.Items[listBox1.SelectedIndex] + " : " + puanlar[listBox1.SelectedIndex] + " Puana sahip.", "@kodzamani.tk");
                listBox1.SelectedIndex = -1;
            }
            catch { }
            }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yükleniyor();
            MessageBox.Show("Sıralama yenilendi.", "@kodzamani.tk");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
               && !char.IsSeparator(e.KeyChar);
            if (e.KeyChar == '£' || e.KeyChar == '½' ||
               e.KeyChar == '€' || e.KeyChar == '₺' ||
               e.KeyChar == '¨' || e.KeyChar == 'æ' ||
               e.KeyChar == 'ß' || e.KeyChar == '´')
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 33 && (int)e.KeyChar <= 47)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 58 && (int)e.KeyChar <= 64)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 91 && (int)e.KeyChar <= 96)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar >= 123 && (int)e.KeyChar <= 127)
            {
                e.Handled = true;
            }
            if ((int)e.KeyChar == 32)
            {
                e.Handled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Giriş.Visible == true)
                MessageBox.Show("Zaten 'Giriş Yap' sayfasındasınız.", "@kodzamani.tk");
            else
                MessageBox.Show("Üst üste giriş yapamazsınız.", "@kodzamani.tk");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Giriş.Visible == true)
                MessageBox.Show("Lütfen hesabınıza giriş yapın.", "@kodzamani.tk");
            else
            {
                timerkontrol1 = false;
                timerkontrol2 = false;
                dinle.Enabled = false;
                yaz.Enabled = false;
                timer1.Enabled = true;
                oyuncularakarsi.Visible = false;
                bilgisayarakarsı.Visible = true;
                button8.BackColor = Color.Moccasin;
                button9.BackColor = Color.LightSalmon;
                button7.BackColor = Color.LightSalmon;
                sunucusil();
            }
        }
        List<string> sunucular = new List<string>();
        private void sunucularıyenile()
        {
            sunucular.Clear();
            listBox2.Items.Clear();
            baglantı.Open();
            komut = new MySqlCommand();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From SUNUCU");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (oku["oyuncu2"].ToString()=="null")
                    sunucular.Add(oku["kullaniciadi"].ToString() + "|" + oku["sifre"].ToString());
            }
            baglantı.Close();
            foreach (string sunucu in sunucular)
                listBox2.Items.Add(sunucu.Split('|')[0]);
        }
        private void sunucuoluştur()
        {
            baglantı.Open();
            komut = new MySqlCommand();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From SUNUCU");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (oku["kullaniciadi"].ToString() == kullaniciadihesap)
                {
                    baglantı.Close();
                    baglantı.Open();
                    komut = new MySqlCommand("delete from SUNUCU where kullaniciadi = '" + textBox4.Text + "'", baglantı);
                    komut.ExecuteNonQuery();
                    break;
                }
            }
            baglantı.Close();
            string sifremmm = "";
            string sifrehaneleri = "1234567890qazwsxedcrfvtgbyhnujmklopi";
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
                sifremmm += sifrehaneleri[rnd.Next(sifrehaneleri.Length)];
            textBox4.Text = kullaniciadihesap;
            textBox3.Text = sifremmm;
            baglantı.Open();
            komut = new MySqlCommand("insert into SUNUCU (kullaniciadi,sifre,oyuncu1,oyuncu2,oyun1,oyun2) values ('" + textBox4.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + "null" + "','" + "null" + "','" + "null"+ "')", baglantı);
            komut.ExecuteNonQuery();
            baglantı.Close();
            sunucularıyenile();
            Clipboard.SetText(textBox3.Text);
            label12.Text = "Sunucu sahibi : " + kullaniciadihesap;
            label13.Text = kullaniciadihesap + ":0";
            listBox3.Items.Insert(0, "Sunucunuz başarıyla oluşturuldu.");
            //MessageBox.Show("Sunucunuz oluşturuldu şifreniz : " + sifremmm + " Kopyalandı.", "@kodzamani.tk");
        }
        private void sifredegistir()
        {
            if (textBox3.Text == "")
                textBox3.Text = "null";
            textBox4.Text = kullaniciadihesap;
            baglantı.Open();
            komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + kullaniciadihesap + "' , sifre='" + textBox3.Text + "',oyuncu1='" + "null" + "',oyuncu2='" + "null" + "',oyun1='" + "null" + "',oyun2='" + "null" + "' where kullaniciadi='" + kullaniciadihesap + "'";
            komut.ExecuteNonQuery();
            komut.Dispose();
            baglantı.Close();
            sunucularıyenile();
            Clipboard.SetText(textBox3.Text);
            listBox3.Items.Insert(0, "Şifreniz başarıyla değiştirildi");
            }
        private void sunucusil()
        {
            try
            {
                baglantı.Open();
                komut = new MySqlCommand("delete from SUNUCU where kullaniciadi = '" + kullaniciadihesap + "'", baglantı);
                komut.ExecuteNonQuery();
                baglantı.Close();
            }
            catch { }
        }
        private void baglan()
        {
            if (textBox4.Text != kullaniciadihesap)
            {
                bool baglan = false;
                baglantı.Open();
                komut = new MySqlCommand();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From SUNUCU");
                MySqlDataReader oku = komut.ExecuteReader();
                if (textBox3.Text == "")
                    textBox3.Text = "null";
                while (oku.Read())
                {
                    if (oku["kullaniciadi"].ToString() == textBox4.Text && oku["sifre"].ToString() == textBox3.Text)
                    {
                        if (oku["oyuncu2"].ToString() != "null")
                        {
                            listBox3.Items.Insert(0, "Sunucuya bağlanılamaz. Zaten oyun oynanıyor.");
                            sunucularıyenile();
                        }
                        else if (oku["kullaniciadi"].ToString() == kullaniciadihesap)
                        {
                            listBox3.Items.Insert(0, "Sunucuya zaten bağlısınız.");
                            sunucularıyenile();
                        }
                        else
                            baglan = true;
                        break;
                    }
                }
                baglantı.Close();
                if (baglan == true)
                {
                    baglantı.Open();
                    komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + textBox4.Text + "' , sifre='" + textBox3.Text + "',oyuncu1='" + textBox4.Text + "',oyuncu2='" + kullaniciadihesap + "',oyun1='" + "null" + "',oyun2='" + "null" + "' where kullaniciadi='" + textBox4.Text + "'";
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    baglantı.Close();
                    sunucusil();
                    listBox3.Items.Insert(0, "Sunucuya başarıyla bağlanıldı.");
                    label12.Text = "Sunucu sahibi : " + textBox4.Text;
                    label13.Text = textBox4.Text + ":0";
                    label9.Text = kullaniciadihesap + ":0";
                    yaz.Interval = 3000;
                    yaz.Enabled = true;
                    timerkontrol2 = true;
                }
                else
                    listBox3.Items.Insert(0, "Kullaniciadi veya sifre yanlış");
            }
            else
                listBox3.Items.Insert(0, "Sunucuya zaten bağlısınız.");
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (Giriş.Visible == true)
                listBox3.Items.Insert(0, "Lütfen hesabınıza giriş yapın.");
            else
            {
                oyunaslat = false;
                dinle.Enabled = true;
                dinle.Interval = 3000;
                yaz.Enabled = false;
                timerkontrol1 = true;
                timerkontrol2 = false;
                timer1.Enabled = false;
                oyuncularakarsi.Visible = true;
                bilgisayarakarsı.Visible = false;
                button8.BackColor = Color.LightSalmon;
                button9.BackColor = Color.Moccasin;
                button7.BackColor = Color.LightSalmon;
                listBox3.Items.Clear();
                sunucuoluştur();
            }
        }

        private void oyuncularakarsi_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Haydaa elime niye vuruyorsun :(");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (timerkontrol1 == true)
            {
                timerkontrol1 = false;
                dinle.Enabled = false;
                can = true;
            }
            if (timerkontrol2 == true)
            {
                timerkontrol2 = false;
                yaz.Enabled = false;
                can = false;
            }
            sunucularıyenile();
            if (can == true)
            {
                dinle.Enabled = true;
                timerkontrol1 = true;
            }
            else
            {
                yaz.Enabled = true;
                timerkontrol2 = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (timerkontrol1 == true)
            {
                timerkontrol1 = false;
                dinle.Enabled = false;
                can = true;
            }
            if (timerkontrol2 == true)
            {
                timerkontrol2 = false;
                yaz.Enabled = false;
                can = false;
            }
            if (textBox3.Text == "" || textBox3.Text == "null")
            {
                DialogResult dialogResult = MessageBox.Show("Eğer şifrenizi bu şekilde değiştirirseniz sunucunuza herkes erişebilecek. Devam etmek istiyor musunuz ?", "@kodzamani.tk", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    sifredegistir();
                }
            }
            else
                sifredegistir();
            if (can == true)
            {
                dinle.Enabled = true;
                timerkontrol1 = true;
            }
            else
            {
                yaz.Enabled = true;
                timerkontrol2 = true;
            }
        }
        bool can = false;
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timerkontrol2 == true)
            {
                timerkontrol2 = false;
                yaz.Enabled = false;
                can = false;
            }
            if (timerkontrol1 == true)
            {
                timerkontrol1 = false;
                dinle.Enabled = false;
                can = true;
            }
            textBox4.Text = listBox2.Text;
            textBox3.Text = "";
            if (listBox2.Text == kullaniciadihesap)
            {
                baglantı.Open();
                komut = new MySqlCommand();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From SUNUCU");
                MySqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["kullaniciadi"].ToString() == textBox4.Text)
                    {
                        listBox3.Items.Insert(0, "Şifreniz kopyalandı.");
                        textBox3.Text = oku["sifre"].ToString();
                        Clipboard.SetText(oku["sifre"].ToString());
                        break;
                    }
                }
                baglantı.Close();
            }
            else
            {
                baglantı.Open();
                komut = new MySqlCommand();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From SUNUCU");
                MySqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["kullaniciadi"].ToString() == listBox2.Text&& oku["sifre"].ToString()=="null")
                    {
                        textBox3.Text = "null";
                        break;
                    }
                }
                baglantı.Close();
                if (textBox3.Text == "null")
                    baglan();
            }
            if (can == false)
            {
                timerkontrol2 = true;
                yaz.Enabled = true;
            }
            else
            {
                timerkontrol1 = true;
                dinle.Enabled = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sunucusil();
            if (getirildimi == true)
                Process.Start("https://kodzamani.weebly.com");
        }
        bool timerkontrol1 = false;
        bool timerkontrol2 = false;
        private void button12_Click(object sender, EventArgs e)
        {
            dinle.Enabled = false;
            timerkontrol1 = false;
            timerkontrol2 = true;
            baglan();
        }
        bool oyunaslat = false;
        string sunucununsifresi = "";
        private void dinle_Tick(object sender, EventArgs e)
        {
            durumlarabak();
            if (oyunaslat == false)
            {
                baglantı.Open();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From SUNUCU");
                MySqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["kullaniciadi"].ToString() == label13.Text.Split(':')[0])
                    {
                        if (oku["oyuncu2"].ToString() != "null")
                        {
                            label9.Text = oku["oyuncu2"].ToString()+":0";
                            oyunaslat = true;
                            sunucununsifresi = oku["sifre"].ToString();
                            break;
                        }
                    }
                }
                baglantı.Close();
                if (oyunaslat == true)
                {
                    Random rnd = new Random();
                    int oyna = rnd.Next(2);
                    baglantı.Open();
                    if (oyna == 1)
                        komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + label13.Text.Split(':')[0] + "' , sifre='" + sunucununsifresi + "',oyuncu1='" + label13.Text.Split(':')[0] + "',oyuncu2='" + label9.Text.Split(':')[0] + "',oyun1='" + "oyna" + "',oyun2='" + "null" + "' where kullaniciadi='" + label13.Text.Split(':')[0] + "'";
                    else
                        komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + label13.Text.Split(':')[0] + "' , sifre='" + sunucununsifresi + "',oyuncu1='" + label13.Text.Split(':')[0] + "',oyuncu2='" + label9.Text.Split(':')[0] + "',oyun1='" + "null" + "',oyun2='" + "oyna" + "' where kullaniciadi='" + label13.Text.Split(':')[0] + "'";
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    baglantı.Close();
                    listBox3.Items.Insert(0, label9.Text.Replace(":0", "") + " Sunucunuza katıldı");
                }
            }
            else
            {
                bool contra = false;
                baglantı.Open();
                komut = new MySqlCommand();
                komut.Connection = baglantı;
                komut.CommandText = ("Select * From SUNUCU");
                MySqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    if (oku["oyuncu1"].ToString() == label13.Text.Split(':')[0]&& oku["oyuncu2"].ToString() == label9.Text.Split(':')[0])
                    {
                        if (oku["oyun1"].ToString().Contains("oyna") == true)
                        {
                            contra = true;
                            break;
                        }
                    }
                }
                baglantı.Close();
                if (contra == true)
                {
                    if (listBox3.Items[0] != "Oynamanız bekleniyor.")
                        listBox3.Items.Insert(0, "Oynamanız bekleniyor.");
                    groupBox4.Enabled = true;
                    dinle.Enabled = false;
                    timerkontrol1 = false;
                }
                else
                {
                    if (listBox3.Items[0]!= "Rakip bekleniyor.")
                    listBox3.Items.Insert(0, "Rakip bekleniyor.");
                    groupBox4.Enabled = false;
                    dinle.Enabled = true;
                    timerkontrol1 = false;
                }
                //sunucu sahibi oynaması
            }
            sunucularıyenile();
        }
        bool OYY = false;
        private void oyna(string veri)
        {
            bool connn = false;
            groupBox4.Enabled = false;
            string oyunverileri = "";
            string oyunverileri2 = "";
            baglantı.Open();
            komut = new MySqlCommand();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From SUNUCU");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (oku["kullaniciadi"].ToString() == label13.Text.Split(':')[0])
                {
                    if (oku["oyun1"].ToString().Contains("oyna") == true)
                    {
                        oyunverileri = oku["oyun1"].ToString().Replace("oyna", "").Replace("null:", "");
                        oyunverileri2 = oku["oyun2"].ToString().Replace("null:","");
                        OYY = true;
                        connn = true;
                        break;
                    }
                    if (oku["oyun2"].ToString().Contains("oyna") == true)
                    {
                        oyunverileri = oku["oyun1"].ToString().Replace("null:", "");
                        oyunverileri2 = oku["oyun2"].ToString().Replace("oyna", "").Replace("null:", "");
                        OYY = false; 
                        connn = true;
                        break;
                    }
                }
            }
            baglantı.Close();
            if (connn == true)
            {
                baglantı.Open();
                if (OYY == true)
                {
                    komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + label13.Text.Split(':')[0] + "' , sifre='" + sunucununsifresi + "',oyuncu1='" + label13.Text.Split(':')[0] + "',oyuncu2='" + label9.Text.Split(':')[0] + "',oyun1='" + oyunverileri + ":" + veri + "',oyun2='" + oyunverileri2 + ":oyna" + "' where kullaniciadi='" + label13.Text.Split(':')[0] + "'";
                    dinle.Enabled = true;
                    yaz.Enabled = false;
                    timerkontrol1 = true;
                    timerkontrol2 = false;
                }
                else
                {
                    komut.CommandText = "UPDATE SUNUCU SET kullaniciadi='" + label13.Text.Split(':')[0] + "' , sifre='" + sunucununsifresi + "',oyuncu1='" + label13.Text.Split(':')[0] + "',oyuncu2='" + label9.Text.Split(':')[0] + "',oyun1='" + oyunverileri + ":oyna" + "',oyun2='" + oyunverileri2 + ":" + veri + "' where kullaniciadi='" + label13.Text.Split(':')[0] + "'";
                    yaz.Enabled = true;
                    dinle.Enabled = false;
                    timerkontrol1 = false;
                    timerkontrol2 = true;
                }
                komut.ExecuteNonQuery();
                komut.Dispose();
                baglantı.Close();
            }
            durumlarabak();
        }
        private void durumlarabak()
        {
            string oyun1veri = "";
            string oyun2veri = "";
            baglantı.Open();
            komut = new MySqlCommand();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From SUNUCU");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (oku["kullaniciadi"].ToString() == label12.Text.Replace("Sunucu sahibi : ", ""))
                {
                    oyun1veri = oku["oyun1"].ToString();
                    oyun2veri = oku["oyun2"].ToString();
                    break;
                }
            }
            baglantı.Close();
            string[] oyun1veriler = oyun1veri.Split(':');
            string[] oyun2veriler = oyun2veri.Split(':');
            int bsayac = 0;
            int osayac = 0;
            for (int i=1;i< oyun1veriler.Length;i++)
            {
                try
                {
                    string b = oyun1veriler[i];
                    string o = oyun2veriler[i];
                    if (b == "t" && o == "k")
                        osayac++;
                    if (b == "t" && o == "m")
                        bsayac++;

                    if (b == "k" && o == "t")
                        bsayac++;
                    if (b == "k" && o == "m")
                        osayac++;

                    if (b == "m" && o == "t")
                        osayac++;
                    if (b == "m" && o == "k")
                        bsayac++;
                }
                catch { }
            }
            label13.Text = label13.Text.Split(':')[0] +":"+ bsayac;
            label9.Text = label9.Text.Split(':')[0] +":"+ osayac;
            if (osayac == 3)
            {
                timerkontrol1 = false;
                dinle.Enabled = false;
                timerkontrol2 = false;
                yaz.Enabled = false;
                MessageBox.Show("Oyunu Kazanan : " + label9.Text.Split(':')[0], "@kodzamani.tk");
                label9.Text = "null:0";
                label12.Text = "Sunucu sahibi : " + kullaniciadihesap;
                button8.PerformClick();
                button9.PerformClick();
            }
            if (bsayac == 3)
            {
                timerkontrol1 = false;
                dinle.Enabled = false;
                timerkontrol2 = false;
                yaz.Enabled = false;
                MessageBox.Show("Oyunu Kazanan : " + label13.Text.Split(':')[0],"@kodzamani.tk");
                label9.Text = "null:0";
                label12.Text = "Sunucu sahibi : "+kullaniciadihesap;
                button8.PerformClick();
                button9.PerformClick();
            }
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            oyna("t");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            oyna("m");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            oyna("k");
        }

        private void yaz_Tick(object sender, EventArgs e)
        {
            durumlarabak();
            bool contra = false;
            baglantı.Open();
            komut.Connection = baglantı;
            komut.CommandText = ("Select * From SUNUCU");
            MySqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (oku["oyuncu1"].ToString() == label13.Text.Split(':')[0] && oku["oyuncu2"].ToString() == label9.Text.Split(':')[0])
                {
                    if (oku["oyun2"].ToString().Contains("oyna") == true)
                    {
                        contra = true;
                        break;
                    }
                }
            }
            baglantı.Close();
            if (contra == true)
            {
                if (listBox3.Items[0] != "Oynamanız bekleniyor.")
                    listBox3.Items.Insert(0, "Oynamanız bekleniyor.");
                groupBox4.Enabled = true;
                yaz.Enabled = false;
                timerkontrol2 = false;
            }
            else
            {
                if (listBox3.Items[0] != "Rakip bekleniyor.")
                    listBox3.Items.Insert(0, "Rakip bekleniyor.");
                groupBox4.Enabled = false;
                yaz.Enabled = true;
                timerkontrol2 = true;
            }
            sunucularıyenile();
        }
    }
}
