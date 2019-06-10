using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
namespace _3Des_attempt1
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Эта функция преобразовывает файл любого (указанного в параметрах) формата в байтовую последовательность 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private int[] GetBinaryFile(string filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
            }
            BitArray bits = new BitArray(bytes);//заводим битовый массив для дальнейшей его кодировки (содержание - последовательность true и false)
            BitArray bitsNEW = new BitArray(bytes);//аналогичный битовый массив на всякий пожарный

            int[] iint = new int[bits.Length];//массив целых чисел для получения единичнонулевой последовательности
            for (int i = 0; i < bits.Length ; i++)//заполняем массив бинарной последоавтельностью
                iint[i] = Convert.ToInt16(bits[i]);
            return iint;
        }
        /// <summary>
        /// Эта функция преобразовывает байтовый массив в файл указанного вручную формата 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private void GetNEWFile(int[] iint, string filename)
        {
            BitArray bitsNEW = new BitArray(iint.Length);
            for (int i = 0; i < iint.Length; i++)//делаем обратное преобразование в последовательность тру и фолсов
                bitsNEW[i] = Convert.ToBoolean(iint[i]);

            bool l = bitsNEW[5];//черновичок

            byte[] file_final = new byte[bitsNEW.Length / 8];//делаем финальный байтовый массив
            bitsNEW.CopyTo(file_final, 0);//копируем битовый массив в байтовый
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                file.Write(file_final, 0, (int)file_final.Length);
            }
        }

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "фото.jpg";
            MessageBox.Show("All three keys are saved in 'bin' folder as .txt files.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// здесь заводим все массивы и строки, чтобы при выполнении функций не выделять под них память много раз
        /// </summary>
        int[,] ss = new int[4, 16];//s

        int[] L = new int[32]; int[] R = new int[32]; int[] LR = new int[32];//crypt

        int[] bita41 = new int[4]; int[] bita42 = new int[4]; int[] bita43 = new int[4]; int[] bita44 = new int[4]; int[] bita45 = new int[4]; int[] bita46 = new int[4]; int[] bita47 = new int[4]; int[] bita48 = new int[4];//f
        int[,] s1 = new int[4, 16]; int[,] s2 = new int[4, 16]; int[,] s3 = new int[4, 16]; int[,] s4 = new int[4, 16]; int[,] s5 = new int[4, 16]; int[,] s6 = new int[4, 16]; int[,] s7 = new int[4, 16]; int[,] s8 = new int[4, 16];
        int[] b1 = new int[6]; int[] b2 = new int[6]; int[] b3 = new int[6]; int[] b4 = new int[6]; int[] b5 = new int[6]; int[] b6 = new int[6]; int[] b7 = new int[6]; int[] b8 = new int[6];


        string str, str1, str2, str3,str4;//для Е P Ki
        string[] STR, STR1, STR2, STR3, STR4;//для Е P Ki
        string ipstr, ipstr2;//IP
        string[] IPSTR, IPSTR2;//IP

        string SS1, SS2, SS3, SS4, SS5, SS6, SS7, SS8;//для блоков S
        string[] SSS1, SSS2, SSS3, SSS4, SSS5, SSS6, SSS7, SSS8;//для блоков S



        /// <summary>
        /// приводит изначальную последовательность битов в нужный вид (чтобы длина была кратна 64-м)
        /// а также делает из последовательности битов список, с массивами длиной 64
        /// </summary>
        /// <param name="iint"></param>
        /// <returns></returns>
        public List <int[]>PRIVEDENIE(int[]iint)
        {
            int schetchik = 0;
            while ((iint.Length+schetchik)%64!=0)
            {
                schetchik++;
            }
            int[] promej = new int[iint.Length + schetchik];
            for (int i = 0; i < iint.Length; i++)
                promej[i + schetchik] = iint[i];
            List<int[]> blocks = new List<int[]>();
            int howmuchelements = 64;
            for (int j = 0; j < promej.Length; j += howmuchelements)
                blocks.Add(promej.Skip(j).Take(howmuchelements).ToArray());
            int[] priv = blocks[0];
            return blocks;
        }//юзлесс херня работающая 40 секунд (говно ваши листы)
        /// <summary>
        /// функция расширения до 48 бит
        /// </summary>
        /// <param name="Side"></param>
        /// <returns></returns>
        public int[] E (int[] Side)//делается по таблице(2) из вики
        {
            int[] iint2 = new int[48];//сюда запишем таблицу из файла
            int[] iint3 = new int[48];//сюда запишем конечную последовательность, 
                                      //в которой элементы переставлены согласно таблице
            for (int i = 0; i < 48; i++)//записываем таблицу
                iint2[i] = Convert.ToInt32(STR[i]);//переставляем элементы местами
            for (int i = 0; i < 48; i++)
                iint3[i] = Side[iint2[i] - 1];//-1 потому что в таблице нумерация начинается с единицы, а нам надо с 0
            return iint3;
        }
        /// <summary>
        /// промежуточная перестановка после блоков s
        /// </summary>
        public int[] P (int[]bita41, int[] bita42, int[] bita43, int[] bita44, int[] bita45, int[] bita46, int[] bita47, int[] bita48)
        {
            int[] iint2 = new int[48];//сюда запишем таблицу из файла
            int[] iint3 = new int[48];//сюда запишем конечную последовательность, 
                                      //в которой элементы переставлены согласно таблице
            int[] promej = new int[32];//промежуточный массив для соединения всех блоков в один
            for (int i=0,j=0;i<32;i++,j++)//соединяем все блоки в один
            {
                if (j == 4) j -= 4;//переменная j для массива длиной 4 (4битового), поэтому нужно каждый раз обнулять 
                if (i < 4)
                    promej[i] = bita41[j];
                if (i>=4&&i<8)
                    promej[i] = bita42[j];
                if (i >= 8 && i < 12)
                    promej[i] = bita43[j];
                if (i >= 12 && i < 16)
                    promej[i] = bita44[j];
                if (i >= 16 && i < 20)
                    promej[i] = bita45[j];
                if (i >= 20 && i < 24)
                    promej[i] = bita46[j];
                if (i >= 24 && i < 28)
                    promej[i] = bita47[j];
                if (i >= 28 && i < 32)
                    promej[i] = bita48[j];
            }
            for (int i = 0; i < 32; i++)//считываем таблицу
                iint2[i] = Convert.ToInt32(STR1[i]);
            for (int i = 0; i < 32; i++)//производим перестановку
                iint3[i] = promej[iint2[i] - 1];//-1 потому что в таблице нумерация начинается с единицы, а нам надо с 0
            return iint3;
        }//делается по таблице(4) из вики
         /// <summary>
         /// получение всех ключе по заданному ключу из файла
         /// </summary>
         /// <param name="i"></param>
         /// <returns></returns>
        public int[][] Ki(string filename)
        {
            int[] Key = GetBinaryFile(filename);//считываем исходный ключ из файла
            int[] sdvig = new int[16];
            int[] C0 = new int[28];//левая половина
            int[] D0 = new int[28];//правая половина
            int[] sdv_i_tyi = new int[56]; //массив со сдвигами для i-го шага
            int[] result_Ki = new int[48];
            //массив массивов для хранения ключей
            int[][] jaggedArray = new int[16][]; jaggedArray[0] = new int[48]; jaggedArray[1] = new int[48]; jaggedArray[2] = new int[48]; jaggedArray[3] = new int[48]; jaggedArray[4] = new int[48]; jaggedArray[5] = new int[48]; jaggedArray[6] = new int[48]; jaggedArray[7] = new int[48]; jaggedArray[8] = new int[48]; jaggedArray[9] = new int[48]; jaggedArray[10] = new int[48]; jaggedArray[11] = new int[48]; jaggedArray[12] = new int[48]; jaggedArray[13] = new int[48]; jaggedArray[14] = new int[48]; jaggedArray[15] = new int[48];

            for (int j = 0; j < 28; j++)
                C0[j] = Convert.ToInt32(STR2[j]) - 1;
            for (int j = 28; j < 56; j++)
                D0[j - 28] = Convert.ToInt32(STR2[j]) - 1;//разбиение ключа на 2 части


            for (int j = 0; j < 16; j++)
                sdvig[j] = Convert.ToInt32(STR3[j]); //берем таблицу количества сдвигов для итой операции

            for (int i = 0; i < 15; i++)
            {
                int[] C_ityi = C0;//ввожу две переменные, чтобы можно было потом в изначальные внести  эти две промежуточные
                int[] D_ityi = D0;

                int tmp_sdvig = 0;//делаем сдвиг для обеих частей
                for (int k = 0; k < sdvig[i]; k++)
                {
                    tmp_sdvig = C0[0];
                    for (int j = 0; j < 27; j++)
                        C_ityi[j] = C_ityi[j + 1];
                    C_ityi[27] = tmp_sdvig;
                }
                for (int k = 0; k < sdvig[i]; k++)
                {
                    tmp_sdvig = D_ityi[0];
                    for (int j = 0; j < 27; j++)
                        D_ityi[j] = D_ityi[j + 1];
                    D_ityi[27] = tmp_sdvig;
                }

                for (int j = 0; j < 28; j++)//соединяем обратно
                    sdv_i_tyi[j] = C_ityi[j];
                for (int j = 28; j < 56; j++)
                    sdv_i_tyi[j] = D_ityi[j - 28];

                int[] tabl7 = new int[48];//получаем итый ключ

                for (int j = 0; j < 48; j++)
                    tabl7[j] = Convert.ToInt32(STR4[j]) - 1;
                for (int j = 0; j < 48; j++)
                    result_Ki[j] = Key[sdv_i_tyi[tabl7[j]]];
                jaggedArray[i] = result_Ki;
                C0 = C_ityi;//запоминаем получившиеся половинки
                D0 = D_ityi;//
            }
            return jaggedArray; //возвращаем массив массивов с ключами
        }
        /// <summary>
        /// функция считывать блок S из текстового файла 
        /// </summary>
        /// <param name="S"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int[,] s(int[,]S, string[] SSSS)
        {
            //int[,] ss = new int[4, 16];
            int k = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 16; j++)
                    ss[i, j] = Convert.ToInt32(SSSS[k++]);
            return ss;
        }
        /// <summary>
        /// функция делает первоначальную и конечную перестановки для 64 битного блока по таблице (1) из вики
        /// </summary>
        /// <param name="iint"></param>
        /// <returns></returns>
        public int[] IP(int[]iint,string[]IPP)
        {
            int[] iint2 = new int[iint.Length];//сюда запишем таблицу из файла
            int[] iint3 = new int[iint.Length];//сюда запишем конечную последовательность, 
                                               //в которой элементы переставлены согласно таблице
            for (int i=0;i<iint.Length;i++)//записываем таблицу
                iint2[i] = Convert.ToInt32(IPP[i]);
            for (int i = 0; i < iint.Length; i++)//переставляем элементы местами
                iint3[i] = iint[iint2[i]-1];//-1 потому что в таблице нумерация начинается с единицы, а нам надо с 0
            return iint3;
        }
        /// <summary>
        /// DESшифрование для блока 64
        /// </summary>
        /// <param name="iint"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int[] crypt(int[]iint, int[][] keys)
        {
            //int[][] keys=Ki();
            iint = IP(iint, IPSTR);//первоначальная перестановка по таблице
            //int[] L = new int[32]; int[] R = new int[32]; int[] LR = new int[32];
            int[] promej = new int[32];
            for (int j = 0; j < 32; j++) L[j] = iint[j];//левая половина входного блока
            for (int j = 32; j < 64; j++) R[j-32] = iint[j];//правая половина входного блока

            for (int i=0;i<16;i++)//16 циклов преобразования
            {
                for (int k = 0; k < L.Length; k++)//согласно схеме, левая часть блока следующей итерации равна правой части блока текущей итерации
                {
                    LR[k] = L[k];
                    L[k] = R[k];
                }
                promej = F(R, keys[i]);//преобразование функцией фейстеля
                for (int j = 0; j < 32; j++)//правая часть блока следующей итерации равна XOR'у левой части предыдущего с promej 
                    R[j] = LR[j] ^ promej[j];
                int x = 0;
            }
            for (int j = 0; j < 32; j++) iint[j] = L[j]; for (int j = 32; j < 64; j++) iint[j] = R[j-32];//склеиваем зашифрованные левую и правые части воедино
            iint = IP(iint, IPSTR2);//конечная перестановка по таблице
            return iint;

        }
        /// <summary>
        /// DES-расшифрование для блока 64
        /// </summary>
        /// <param name="iint"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int[] decrypt(int[] iint,int[][]keys)
        {
            //int[][] keys = Ki();
            iint = IP(iint, IPSTR);//первоначальная перестановка по таблице
            //int[] L = new int[32]; int[] R = new int[32]; int[] LR = new int[32]; 
            int[] promej = new int[32];
            for (int j = 0; j < 32; j++) L[j] = iint[j];//левая половина входного блока
            for (int j = 32; j < 64; j++) R[j - 32] = iint[j];//правая половина входного блока

            for (int i = 15; i > -1; i--)//16 циклов преобразования
            {
                for (int k = 0; k < L.Length; k++)//согласно схеме, правая часть блока следующей итерации равна левой части блока текущей итерации
                {
                    //LR = L;
                    LR[k] = R[k];
                    //L = R;
                    R[k] = L[k];
                }

                promej = F(L, keys[i]);//преобразование функцией фейстеля
                for (int j = 0; j < 32; j++)
                    L[j] = LR[j] ^ promej[j];//левая часть блока следующей итерации равна XOR'у правой части предыдущего с promej
            }
            for (int j = 0; j < 32; j++) iint[j] = L[j]; for (int j = 32; j < 64; j++) iint[j] = R[j - 32];//склеиваем расшифрованные левую и правые части воедино
            iint = IP(iint, IPSTR2);//конечная перестановка по таблице
            return iint;

        }
        /// <summary>
        /// функция пребразования для циклов шифрования, зависит от входного блока(32бит) и Итого ключа(48 бит)
        /// </summary>
        /// <param name="Side"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public int[] F(int[] Side, int[]Key)
        {
            int[] result = new int[32];//сюда в конце запишем преобразованную последовательность
            int[] promej = new int[48];//промежуточный массив
            //int[] bita41 = new int[4]; int[] bita42 = new int[4]; int[] bita43 = new int[4]; int[] bita44 = new int[4]; int[] bita45 = new int[4]; int[] bita46 = new int[4]; int[] bita47 = new int[4]; int[] bita48 = new int[4];

            //int[,] s1 = new int[4, 16]; int[,] s2 = new int[4, 16]; int[,] s3 = new int[4, 16]; int[,] s4 = new int[4, 16]; int[,] s5 = new int[4, 16]; int[,] s6 = new int[4, 16]; int[,] s7 = new int[4, 16]; int[,] s8 = new int[4, 16];
            //s1 = s(s1, SSS1); s2 = s(s2, SSS2); s3 = s(s3, SSS3); s4 = s(s4, SSS4); s5 = s(s5,SSS5); s6 = s(s6, SSS6); s7 = s(s7, SSS7); s8 = s(s8, SSS8);

            promej = E(Side);//1) РАСШИРЕНИЕ ДО 48 БИТ
            for (int i = 0; i < 48; i++)//2) СЛОЖЕНИЕ ХОР С i-тым КЛЮЧОМ размером 48 бит
                promej[i] = promej[i] ^ Key[i];

            //int[] b1 = new int[6]; int[] b2 = new int[6]; int[] b3 = new int[6]; int[] b4 = new int[6]; int[] b5 = new int[6]; int[] b6 = new int[6]; int[] b7 = new int[6]; int[] b8 = new int[6];
            for (int i = 0,j=0; i < 48; i++,j++)//разбиение на блоки по 6 бит, а также 3) НАЧАЛО ПРЕОБРАЗОВАНИЯ S
            {
                if (j == 6) j -= 6;
                if (i < 6) b1[j] = promej[i]; if (i >= 6 && i < 12) b2[j] = promej[i]; if (i >= 12 && i < 18) b3[j] = promej[i]; if (i >= 18 && i < 24) b4[j] = promej[i]; if (i >= 24 && i < 30) b5[j] = promej[i]; if (i >= 30 && i < 36) b6[j] = promej[i]; if (i >= 36 && i < 42) b7[j] = promej[i]; if (i >= 42 && i < 48) b8[j] = promej[i];
            }


            //int m = b1[5] + 2 * b1[0];
            //int n = b1[4] + 2 * b1[3] + 2 * 2 * b1[2] + 2 * 2 * 2 * b1[1];

            int S1_to4bit = s1[b1[5] + 2 * b1[0], b1[4] + 2 * b1[3] + 2 * 2 * b1[2] + 2 * 2 * 2 * b1[1]];
            int S2_to4bit = s2[b2[5] + 2 * b2[0], b2[4] + 2 * b2[3] + 2 * 2 * b2[2] + 2 * 2 * 2 * b2[1]];
            int S3_to4bit = s3[b3[5] + 2 * b3[0], b3[4] + 2 * b3[3] + 2 * 2 * b3[2] + 2 * 2 * 2 * b3[1]];
            int S4_to4bit = s4[b4[5] + 2 * b4[0], b4[4] + 2 * b4[3] + 2 * 2 * b4[2] + 2 * 2 * 2 * b4[1]];
            int S5_to4bit = s5[b5[5] + 2 * b5[0], b5[4] + 2 * b5[3] + 2 * 2 * b5[2] + 2 * 2 * 2 * b5[1]];
            int S6_to4bit = s6[b6[5] + 2 * b6[0], b6[4] + 2 * b6[3] + 2 * 2 * b6[2] + 2 * 2 * 2 * b6[1]];
            int S7_to4bit = s7[b7[5] + 2 * b7[0], b7[4] + 2 * b7[3] + 2 * 2 * b7[2] + 2 * 2 * 2 * b7[1]];
            int S8_to4bit = s8[b8[5] + 2 * b8[0], b8[4] + 2 * b8[3] + 2 * 2 * b8[2] + 2 * 2 * 2 * b8[1]];//десятичное число, найденное в таблице sj, которое необходимо перевести в 4битовый вид
            for (int i=3;i>-1;i--)// 3) ПРОДОЛЖЕНИЕ ПРЕОБРАЗОВАНИЯ S
            {
                bita41[i] = S1_to4bit % 2; S1_to4bit = S1_to4bit / 2;
                bita42[i] = S2_to4bit % 2; S2_to4bit = S2_to4bit / 2;
                bita43[i] = S3_to4bit % 2; S3_to4bit = S3_to4bit / 2;
                bita44[i] = S4_to4bit % 2; S4_to4bit = S4_to4bit / 2;
                bita45[i] = S5_to4bit % 2; S5_to4bit = S5_to4bit / 2;
                bita46[i] = S6_to4bit % 2; S6_to4bit = S6_to4bit / 2;
                bita47[i] = S7_to4bit % 2; S7_to4bit = S7_to4bit / 2;
                bita48[i] = S8_to4bit % 2; S8_to4bit = S8_to4bit / 2;// 4битовые массивы, прошедшие через s блоки
            }
            result = P(bita41, bita42, bita43, bita44, bita45, bita46, bita47, bita48);//4) ПЕРЕСТАНОВКА P для 4битовых блоков
            return result;

        }

        /// <summary>
        /// DES-шифрование для произвольной последовательности инт
        /// </summary>
        /// <param name="iint"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public int[] DES_crypt (int[]iint, int[][]k)
        {
            int[] odinblock = new int[64];

            for (int l = 0, m = 0; l < iint.Length; l++)//идем по всей битовой последовательности, шифруем каждые 64элемента, сразу записываем обратно
            {
                odinblock[m] = iint[l];
                m++;
                if (m == 64)
                {
                    odinblock = crypt(odinblock, k);
                    m = 0;
                    for (int u = l - 63, f = 0; u <= l; u++, f++)
                        iint[u] = odinblock[f];
                }
            }
            return iint;
        }
        /// <summary>
        /// DES-расшифрование для произвольной последовательности инт
        /// </summary>
        /// <param name="iint"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public int[] DES_decrypt(int[] iint, int[][] k)
        {
            int[] odinblock = new int[64];

            for (int l = 0, m = 0; l < iint.Length; l++)//идем по всей битовой последовательности, расшифровываем каждые 64элемента, сразу записываем обратно
            {
                odinblock[m] = iint[l];
                m++;
                if (m == 64)
                {
                    odinblock = decrypt(odinblock, k);
                    m = 0;
                    for (int u = l - 63, f = 0; u <= l; u++, f++)
                        iint[u] = odinblock[f];
                }
            }
            return iint;
        }
        public int[] trippledES_EDE3_crypt(int[]iint,int[][]k1, int[][] k2, int[][] k3)
        {
            iint = DES_crypt(iint, k1);
            iint = DES_decrypt(iint, k2);
            iint = DES_crypt(iint, k3);

            return iint;
        }
        public int[] trippledES_EDE3_decrypt(int[] iint, int[][] k1, int[][] k2, int[][] k3)
        {
            iint = DES_decrypt(iint, k3);
            iint = DES_crypt(iint, k2);
            iint = DES_decrypt(iint, k1);

            return iint;
        }
        public int[] trippledES_EEE3_crypt(int[] iint, int[][] k1, int[][] k2, int[][] k3)
        {
            iint = DES_crypt(iint, k1);
            iint = DES_crypt(iint, k2);
            iint = DES_crypt(iint, k3);

            return iint;
        }
        public int[] trippledES_EEE3_decrypt(int[] iint, int[][] k1, int[][] k2, int[][] k3)
        {
            iint = DES_decrypt(iint, k3);
            iint = DES_decrypt(iint, k2);
            iint = DES_decrypt(iint, k1);

            return iint;
        }
        bool flag = false;
        /// <summary>
        /// Здесь производится шифрование и дешифрование 3des-ede3,при нажатии на кнопку 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (flag)//проверка нажатия на кнопку 3
            {
                DateTime StartTime;//таймер для подсчета выполнения программы
                StartTime = DateTime.Now;             
                int[][] k1 = Ki("key.txt"); int[][] k2 = Ki("key2.txt"); int[][] k3 = Ki("key3.txt");//считаем все ключи
                string filename = textBox1.Text;//считываем имя файла 
                int[] iint = GetBinaryFile(filename);//получаем битовую последовательность из файла
                iint = trippledES_EDE3_crypt(iint,k1,k2,k3);//шифруем

                filename = "EDE3_after_crypt_" + filename;//сохраняем файл, полученный после crypt
                GetNEWFile(iint, filename);
                iint = trippledES_EDE3_decrypt(iint,k1,k2,k3);//дешифруем

                filename = "EDE3_after_decrypt_"+filename;
                GetNEWFile(iint, filename);//сохраняем дешифрованный файл
                //int[] iint2 = new int[65];
                MessageBox.Show("Done! Check 'bin' folder for EDE3.");
                DateTime EndTime = DateTime.Now;

                MessageBox.Show("The time of implementation 3DES_crypt+3DES_encrypt " + (EndTime - StartTime));
            }
            else
            {
                MessageBox.Show("Bad.. bad.. you better click on that button, other way i'm not going to do anything");
            }
           
        }
        /// <summary>
        /// Здесь производится шифрование и дешифрование 3des-eee3,при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                DateTime StartTime;
                StartTime = DateTime.Now;
                int[][] k1 = Ki("key.txt"); int[][] k2 = Ki("key2.txt"); int[][] k3 = Ki("key3.txt");
                string filename = textBox1.Text;
                int[] iint = GetBinaryFile(filename);
                iint = trippledES_EEE3_crypt(iint, k1, k2, k3);

                filename = "EEE3_after_crypt_" + filename;
                GetNEWFile(iint, filename);
                iint = trippledES_EEE3_decrypt(iint, k1, k2, k3);

                filename = "EEE3_after_decrypt_" + filename;
                GetNEWFile(iint, filename);
                //int[] iint2 = new int[65];
                MessageBox.Show("Done! Check 'bin' folder for EEE3.");
                DateTime EndTime = DateTime.Now;

                MessageBox.Show("The time of implementation 3DES_crypt+3DES_encrypt " + (EndTime - StartTime));
            }
            else
            {
                MessageBox.Show("Bad.. bad.. you better click on that button, other way i'm not going to do anything");
            }
        }

        private void button3_Click(object sender, EventArgs e)//все файлстримы делаем тута 
        {
            flag = true;
            FileStream stream = new FileStream("EE_wider.txt", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            str = reader.ReadToEnd();
            STR = str.Split(' ');
            stream.Close();

            stream = new FileStream("P_perestanovka.txt", FileMode.Open);
            reader = new StreamReader(stream);
            str1 = reader.ReadToEnd();
            STR1 = str1.Split(' ');
            stream.Close();

            stream = new FileStream("tabl5_wiki.txt", FileMode.Open);//разбиение исх-го ключа на 2 части
            reader = new StreamReader(stream);
            str2 = reader.ReadToEnd();
            STR2 = str2.Split(' ');
            stream.Close();
            stream = new FileStream("tabl6_wiki.txt", FileMode.Open);//разбиение исх-го ключа на 2 части
            reader = new StreamReader(stream);
            str3 = reader.ReadToEnd();
            STR3 = str3.Split(' ');
            stream.Close();
            stream = new FileStream("tabl7_wiki.txt", FileMode.Open);//разбиение исх-го ключа на 2 части
            reader = new StreamReader(stream);
            str4 = reader.ReadToEnd();
            STR4 = str4.Split(' ');
            stream.Close();

            stream = new FileStream("s1.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS1 = reader.ReadToEnd();
            SSS1 = SS1.Split(' ');
            stream.Close();
            stream = new FileStream("s2.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS2 = reader.ReadToEnd();
            SSS2 = SS2.Split(' ');
            stream.Close();
            stream = new FileStream("s3.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS3 = reader.ReadToEnd();
            SSS3 = SS3.Split(' ');
            stream.Close();
            stream = new FileStream("s4.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS4 = reader.ReadToEnd();
            SSS4 = SS4.Split(' ');
            stream.Close();
            stream = new FileStream("s5.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS5 = reader.ReadToEnd();
            SSS5 = SS5.Split(' ');
            stream.Close();
            stream = new FileStream("s6.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS6 = reader.ReadToEnd();
            SSS6 = SS6.Split(' ');
            stream.Close();
            stream = new FileStream("s7.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS7 = reader.ReadToEnd();
            SSS7 = SS7.Split(' ');
            stream.Close();
            stream = new FileStream("s8.txt", FileMode.Open);//S BLOKI
            reader = new StreamReader(stream);
            SS8 = reader.ReadToEnd();
            SSS8 = SS8.Split(' ');
            stream.Close();

            stream = new FileStream("IP_begin.txt", FileMode.Open);//IP
            reader = new StreamReader(stream);
            ipstr = reader.ReadToEnd();
            IPSTR = ipstr.Split(' ');
            stream.Close();
            stream = new FileStream("IP_last.txt", FileMode.Open);//IP
            reader = new StreamReader(stream);
            ipstr2 = reader.ReadToEnd();
            IPSTR2 = ipstr2.Split(' ');
            stream.Close();

            s1 = s(s1, SSS1); s2 = s(s2, SSS2); s3 = s(s3, SSS3); s4 = s(s4, SSS4); s5 = s(s5, SSS5); s6 = s(s6, SSS6); s7 = s(s7, SSS7); s8 = s(s8, SSS8);

        }
    }
}
