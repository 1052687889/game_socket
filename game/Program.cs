using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace game
{
    class CRC32
    {
        private static string key = "sdhfkshdakfhdsiafisadkfwkle-jio%^&@$%^*&_rfeuwirndkjsaxfjksdahjfjsdoauocxzbbxzmqrwqr";
        static UInt32[] crcTable =
        {
          0x00000000, 0x04c11db7, 0x09823b6e, 0x0d4326d9, 0x130476dc, 0x17c56b6b, 0x1a864db2, 0x1e475005,
          0x2608edb8, 0x22c9f00f, 0x2f8ad6d6, 0x2b4bcb61, 0x350c9b64, 0x31cd86d3, 0x3c8ea00a, 0x384fbdbd,
          0x4c11db70, 0x48d0c6c7, 0x4593e01e, 0x4152fda9, 0x5f15adac, 0x5bd4b01b, 0x569796c2, 0x52568b75,
          0x6a1936c8, 0x6ed82b7f, 0x639b0da6, 0x675a1011, 0x791d4014, 0x7ddc5da3, 0x709f7b7a, 0x745e66cd,
          0x9823b6e0, 0x9ce2ab57, 0x91a18d8e, 0x95609039, 0x8b27c03c, 0x8fe6dd8b, 0x82a5fb52, 0x8664e6e5,
          0xbe2b5b58, 0xbaea46ef, 0xb7a96036, 0xb3687d81, 0xad2f2d84, 0xa9ee3033, 0xa4ad16ea, 0xa06c0b5d,
          0xd4326d90, 0xd0f37027, 0xddb056fe, 0xd9714b49, 0xc7361b4c, 0xc3f706fb, 0xceb42022, 0xca753d95,
          0xf23a8028, 0xf6fb9d9f, 0xfbb8bb46, 0xff79a6f1, 0xe13ef6f4, 0xe5ffeb43, 0xe8bccd9a, 0xec7dd02d,
          0x34867077, 0x30476dc0, 0x3d044b19, 0x39c556ae, 0x278206ab, 0x23431b1c, 0x2e003dc5, 0x2ac12072,
          0x128e9dcf, 0x164f8078, 0x1b0ca6a1, 0x1fcdbb16, 0x018aeb13, 0x054bf6a4, 0x0808d07d, 0x0cc9cdca,
          0x7897ab07, 0x7c56b6b0, 0x71159069, 0x75d48dde, 0x6b93dddb, 0x6f52c06c, 0x6211e6b5, 0x66d0fb02,
          0x5e9f46bf, 0x5a5e5b08, 0x571d7dd1, 0x53dc6066, 0x4d9b3063, 0x495a2dd4, 0x44190b0d, 0x40d816ba,
          0xaca5c697, 0xa864db20, 0xa527fdf9, 0xa1e6e04e, 0xbfa1b04b, 0xbb60adfc, 0xb6238b25, 0xb2e29692,
          0x8aad2b2f, 0x8e6c3698, 0x832f1041, 0x87ee0df6, 0x99a95df3, 0x9d684044, 0x902b669d, 0x94ea7b2a,
          0xe0b41de7, 0xe4750050, 0xe9362689, 0xedf73b3e, 0xf3b06b3b, 0xf771768c, 0xfa325055, 0xfef34de2,
          0xc6bcf05f, 0xc27dede8, 0xcf3ecb31, 0xcbffd686, 0xd5b88683, 0xd1799b34, 0xdc3abded, 0xd8fba05a,
          0x690ce0ee, 0x6dcdfd59, 0x608edb80, 0x644fc637, 0x7a089632, 0x7ec98b85, 0x738aad5c, 0x774bb0eb,
          0x4f040d56, 0x4bc510e1, 0x46863638, 0x42472b8f, 0x5c007b8a, 0x58c1663d, 0x558240e4, 0x51435d53,
          0x251d3b9e, 0x21dc2629, 0x2c9f00f0, 0x285e1d47, 0x36194d42, 0x32d850f5, 0x3f9b762c, 0x3b5a6b9b,
          0x0315d626, 0x07d4cb91, 0x0a97ed48, 0x0e56f0ff, 0x1011a0fa, 0x14d0bd4d, 0x19939b94, 0x1d528623,
          0xf12f560e, 0xf5ee4bb9, 0xf8ad6d60, 0xfc6c70d7, 0xe22b20d2, 0xe6ea3d65, 0xeba91bbc, 0xef68060b,
          0xd727bbb6, 0xd3e6a601, 0xdea580d8, 0xda649d6f, 0xc423cd6a, 0xc0e2d0dd, 0xcda1f604, 0xc960ebb3,
          0xbd3e8d7e, 0xb9ff90c9, 0xb4bcb610, 0xb07daba7, 0xae3afba2, 0xaafbe615, 0xa7b8c0cc, 0xa379dd7b,
          0x9b3660c6, 0x9ff77d71, 0x92b45ba8, 0x9675461f, 0x8832161a, 0x8cf30bad, 0x81b02d74, 0x857130c3,
          0x5d8a9099, 0x594b8d2e, 0x5408abf7, 0x50c9b640, 0x4e8ee645, 0x4a4ffbf2, 0x470cdd2b, 0x43cdc09c,
          0x7b827d21, 0x7f436096, 0x7200464f, 0x76c15bf8, 0x68860bfd, 0x6c47164a, 0x61043093, 0x65c52d24,
          0x119b4be9, 0x155a565e, 0x18197087, 0x1cd86d30, 0x029f3d35, 0x065e2082, 0x0b1d065b, 0x0fdc1bec,
          0x3793a651, 0x3352bbe6, 0x3e119d3f, 0x3ad08088, 0x2497d08d, 0x2056cd3a, 0x2d15ebe3, 0x29d4f654,
          0xc5a92679, 0xc1683bce, 0xcc2b1d17, 0xc8ea00a0, 0xd6ad50a5, 0xd26c4d12, 0xdf2f6bcb, 0xdbee767c,
          0xe3a1cbc1, 0xe760d676, 0xea23f0af, 0xeee2ed18, 0xf0a5bd1d, 0xf464a0aa, 0xf9278673, 0xfde69bc4,
          0x89b8fd09, 0x8d79e0be, 0x803ac667, 0x84fbdbd0, 0x9abc8bd5, 0x9e7d9662, 0x933eb0bb, 0x97ffad0c,
          0xafb010b1, 0xab710d06, 0xa6322bdf, 0xa2f33668, 0xbcb4666d, 0xb8757bda, 0xb5365d03, 0xb1f740b4
        };

        public static uint GetCRC32(byte[] bytes)
        {
            uint crc = GetCRC32Orgin(bytes)^GetCRC32Orgin(Encoding.UTF8.GetBytes(key+"dream"));
            return crc;
        }
        public static uint GetCRC32Orgin(byte[] bytes) {
            uint iCount = (uint)bytes.Length;
            uint crc = 0xFFFFFFFF;
            for (uint i = 0; i < iCount; i++)
            {
                crc = (crc << 8) ^ crcTable[(crc >> 24) ^ bytes[i]];
            }

            return crc;
        }
    }

    class SocketPack {
        public const byte _HEAD_0 = 0x5A;
        public const byte _HEAD_1 = 0xA5;
        public const byte FIRST_BYTE = 0;
        public const byte SECOND_BYTE = 8;
        public const byte THREE_BYTE = 16;
        public const byte FOUR_BYTE = 24;

        public byte head_0;
        public byte head_1;
        public byte count;
        public ushort lenth;
        public byte[] data;
        public uint crc;

        public byte CountToRandom(byte i)
        {
            // a[0] 必须为0
            byte[] a = {0,46, 211, 116, 156, 188, 140, 205, 118, 11, 4, 83, 59, 123, 117, 81, 176,
                        19, 193, 88, 198, 82, 3, 45, 67, 192, 21, 215, 26, 35, 252, 16, 1, 75,
                        47, 186, 250, 138, 143, 227, 212, 167, 30, 55, 175, 102, 28, 121, 10, 23,
                        218, 126, 234, 44, 220, 64, 96, 77, 155, 12, 36, 5, 197, 214, 103, 228,
                        239, 50, 226, 56, 182, 8, 225, 100, 136, 37, 32, 17, 57, 9, 217, 61, 248,
                        84, 142, 53, 221, 168, 222, 237, 238, 111, 114, 152, 216, 69, 87, 135, 204,
                        161, 179, 42, 90, 183, 184, 159, 122, 74, 98, 172, 150, 112, 243, 178, 40,
                        146, 41, 76, 230, 233, 148, 219, 170, 106, 66, 108, 247, 119, 70, 158, 43,
                        18, 62, 120, 163, 24, 134, 254, 94, 141, 15, 242, 224, 206, 232, 20, 180,
                        137, 203, 101, 240, 209, 236, 89, 78, 27, 60, 93, 34, 107, 25, 246, 85, 194,
                        223, 235, 249, 127, 196, 171, 154, 207, 181, 109, 165, 79, 51, 191, 99,
                        251, 6, 71, 208, 13, 91, 174, 68, 14, 130, 202, 245, 166, 177, 131, 2, 210,
                        255, 38, 39, 72, 173, 22, 133, 241, 52, 49, 213, 29, 200, 73, 86, 195, 145,
                        113, 97, 231, 63, 187, 48, 153, 139, 162, 190, 129, 147, 253, 115, 124, 157,
                        229, 125, 164, 185, 151, 7, 65, 160, 149, 104, 128, 244, 92, 54, 189, 95, 169,
                        31, 132, 58, 199, 80, 110, 33, 105, 201, 144};
            return a[i];
        }

        public SocketPack(byte[] _data,byte count=0) {
            this.head_0 = _HEAD_0;
            this.head_1 = _HEAD_1;
            this.count = CountToRandom(count);
            this.lenth = (ushort)_data.Length;
            this.data = _data;
            byte[] temp = new byte[5 + this.lenth];
            temp[0] = this.head_0;
            temp[1] = this.head_1;
            temp[2] = this.count;
            temp[3] = (byte)(this.lenth >> FIRST_BYTE);
            temp[4] = (byte)(this.lenth >> SECOND_BYTE);
            for (int i = 0; i < this.lenth; i++) {
                temp[5 + i] = this.data[i];
            }
            this.crc = game.CRC32.GetCRC32(temp);
        }

        public byte[] StructToBytes() {
            byte[] reval = new byte[9 + this.lenth];
            reval[0] = this.head_0;
            reval[1] = this.head_1;
            reval[2] = this.count;
            reval[3] = (byte)(this.lenth >> FIRST_BYTE);
            reval[4] = (byte)(this.lenth >> SECOND_BYTE);
            for (int i = 0; i < this.lenth; i++)
            {
                reval[5 + i] = this.data[i];
            }
            reval[5 + this.lenth + 0] = (byte)(this.crc >> FIRST_BYTE);
            reval[5 + this.lenth + 1] = (byte)(this.crc >> SECOND_BYTE);
            reval[5 + this.lenth + 2] = (byte)(this.crc >> THREE_BYTE);
            reval[5 + this.lenth + 3] = (byte)(this.crc >> FOUR_BYTE);
            return reval;
        }
        public void Clear() {
            this.head_0 = 0x00;
            this.head_1 = 0x00;
            this.count = 0x00;
            this.data = new byte[0];
            this.lenth = 0x0000;
            this.crc = 0x00000000;
        }
        public void Print() {
            Console.WriteLine("pack.head_0:0x"+this.head_0.ToString("x"));
            Console.WriteLine("pack.head_1:0x"+this.head_1.ToString("x"));
            Console.WriteLine("pack.count:0x"+this.count.ToString("x"));
            Console.WriteLine("pack.lenth:"+this.lenth.ToString());
            Console.Write("pack.data:");
            foreach (byte i in this.data)
            {
                Console.Write("0x" + i.ToString("x") + " ");
            }
            Console.Write("\n");
            Console.WriteLine("pack.crc:0x"+this.crc.ToString("x"));
        }

        public SocketPack BytesToStruct(byte[] bytes_data) {
            SocketPack pack = new SocketPack(new byte[0],this.count);
            pack.head_0 = bytes_data[0];
            pack.head_1 = bytes_data[1];
            pack.count = bytes_data[2];
            pack.lenth = (ushort)(((ushort)((ushort)bytes_data[3]) << FIRST_BYTE) |
                                  ((ushort)((ushort)bytes_data[4]) << SECOND_BYTE));
            for (int i = 0; i < pack.lenth; i++)
            {
                pack.data[i] = bytes_data[5 + i];
            }
            pack.crc = (uint)bytes_data[5 + pack.lenth + 0] << FOUR_BYTE |
                        (uint)bytes_data[5 + pack.lenth + 1] << THREE_BYTE |
                        (uint)bytes_data[5 + pack.lenth + 2] << SECOND_BYTE |
                        (uint)bytes_data[5 + pack.lenth + 3] << FIRST_BYTE;
            return pack;
        }
        public byte[] GetData() {
            return this.data;
        }
    }

    class ManagerSocketPack
    {
        private enum STEP_TYPE
        {
            STEP_HEAD_0,
            STEP_HEAD_1,
            STEP_COUNT,
            STEP_LENTH_0,
            STEP_LENTH_1,
            STEP_DATA,
            STEP_CRC_0,
            STEP_CRC_1,
            STEP_CRC_2,
            STEP_CRC_3,
        }
        protected SocketPack sock_pack;
        private STEP_TYPE state;
        private uint count;
        public byte target_count;
        private List<SocketPack> buffer_list;
        public ManagerSocketPack(){
            this.sock_pack = new SocketPack(new byte[0],this.target_count);
            this.buffer_list = new List<SocketPack>();
            this.target_count = 0;
        }

        public SocketPack CreateSocketPack(byte[] data) {
            SocketPack sk = new SocketPack(data, this.target_count);
            return sk;
        }

        public void HandleBytesData(byte[] bytes_data,int lenth) {
            for (int i=0; i < lenth; i++) {
                byte value = bytes_data[i];
                switch (this.state)
                {
                    case STEP_TYPE.STEP_HEAD_0:
                        if (value == SocketPack._HEAD_0)
                        {
                            this.sock_pack.Clear();
                            this.state = STEP_TYPE.STEP_HEAD_1;
                            this.sock_pack.head_0 = value;
                        }
                        else {
                            this.state = STEP_TYPE.STEP_HEAD_0;
                        }
                        break;
                    case STEP_TYPE.STEP_HEAD_1:
                        if (value == SocketPack._HEAD_1)
                        {
                            this.state = STEP_TYPE.STEP_COUNT;
                            this.sock_pack.head_1=value;
                        }
                        else {
                            this.state = STEP_TYPE.STEP_HEAD_0;
                        }
                        break;
                    case STEP_TYPE.STEP_COUNT:
                        this.sock_pack.count = value;
                        this.state = STEP_TYPE.STEP_LENTH_0;
                        break;
                    case STEP_TYPE.STEP_LENTH_0:
                        this.state = STEP_TYPE.STEP_LENTH_1;
                        this.sock_pack.lenth = (ushort)(this.sock_pack.lenth | (ushort)value << SocketPack.FIRST_BYTE);
                        break;
                    case STEP_TYPE.STEP_LENTH_1:
                        this.sock_pack.lenth = (ushort)(this.sock_pack.lenth | (ushort)value << SocketPack.SECOND_BYTE);
                        this.count = 0;
                        this.sock_pack.data = new byte[this.sock_pack.lenth];
                        if (this.sock_pack.lenth == 0)
                        {
                            this.state = STEP_TYPE.STEP_CRC_0;
                        }
                        else {
                            this.state = STEP_TYPE.STEP_DATA;
                        }
                        break;
                    case STEP_TYPE.STEP_DATA:
                        this.sock_pack.data[this.count]=value;
                        this.count++;
                        if (this.count >= this.sock_pack.lenth) {
                            this.state = STEP_TYPE.STEP_CRC_0;
                            this.sock_pack.crc = 0;
                        }
                        break;
                    case STEP_TYPE.STEP_CRC_0:
                        this.sock_pack.crc = this.sock_pack.crc | ((uint)value << SocketPack.FIRST_BYTE);
                        this.state = STEP_TYPE.STEP_CRC_1;
                        break;
                    case STEP_TYPE.STEP_CRC_1:
                        this.sock_pack.crc = this.sock_pack.crc | ((uint)value << SocketPack.SECOND_BYTE);
                        this.state = STEP_TYPE.STEP_CRC_2;
                        break;
                    case STEP_TYPE.STEP_CRC_2:
                        this.sock_pack.crc = this.sock_pack.crc | ((uint)value << SocketPack.THREE_BYTE);
                        this.state = STEP_TYPE.STEP_CRC_3;
                        break;
                    case STEP_TYPE.STEP_CRC_3:
                        this.sock_pack.crc = this.sock_pack.crc | ((uint)value << SocketPack.FOUR_BYTE);
                        byte[] temp = new byte[5 + this.sock_pack.lenth];
                        temp[0] = this.sock_pack.head_0;
                        temp[1] = this.sock_pack.head_1;
                        temp[2] = this.sock_pack.count;
                        temp[3] = (byte)(this.sock_pack.lenth >> SocketPack.FIRST_BYTE);
                        temp[4] = (byte)(this.sock_pack.lenth >> SocketPack.SECOND_BYTE);
                        for (int j = 0; j < this.sock_pack.lenth; j++)
                        {
                            temp[5 + j] = this.sock_pack.data[j];
                        }
                        if (CRC32.GetCRC32(temp) == this.sock_pack.crc) {
                            this.buffer_list.Add(this.sock_pack);
                            this.sock_pack = new SocketPack(new byte[0],this.target_count);
                        }
                        this.state = STEP_TYPE.STEP_HEAD_0;
                        break;

                    default:
                        this.state = STEP_TYPE.STEP_HEAD_0;
                        break;
                }
            }
        }
        public uint GetBufferLenth() {
            return (uint)this.buffer_list.Count;
        }
        public bool GetPack(out SocketPack sk) {
            if (this.buffer_list.Count == 0)
            {
                sk = new SocketPack(new byte[0]);
                return false;
            }
            else {
                sk = this.buffer_list.First();
                this.buffer_list.Remove(sk);
                return true;
            }
}
    }





    class TcpClient
    {
        private enum SYS_MSG_TYPE:ushort{
            HEART_BREAK = 0x0000,
            SET_COUNT = 0x0001,
        }
        public bool isConnect = false;
        private const byte SYS_TYPE_COUNT = 0x00;
        private string host;
        private int port;
        private Socket sock;
        private Thread RevThread;
        private Thread ClientSysThread;
        private byte[] read_buffer;
        private ManagerSocketPack dpack;
        
        public TcpClient(string host,int port)
        {
            this.host = host;
            this.port = port;
            this.read_buffer = new byte[100];
            this.dpack = new ManagerSocketPack();
        }
        private void HandleRevData()
        {
            int lenth = 0;
            while (true)
            {
                try
                {
                    lenth = this.sock.Receive(this.read_buffer, this.read_buffer.Length, 0);
                    if (lenth > 0)
                    {
                        this.dpack.HandleBytesData(this.read_buffer, lenth);
                    }
                }
                catch {
                    this.isConnect = false;
                    this.sock.Close();
                    break;
                }
            }
        }

        private byte[] CreateSysPack(SYS_MSG_TYPE _type,byte[] data) {
            byte[] reval = new byte[sizeof(SYS_MSG_TYPE) + data.Length];
            reval[0] = (byte)((ushort)_type >> SocketPack.FIRST_BYTE);
            reval[1] = (byte)((ushort)_type >> SocketPack.SECOND_BYTE);
            for (int i = 0; i < data.Length; i++) {
                reval[2 + i] = data[i];
            }
            return reval;
        }

        
        private bool SendSysMsg(SYS_MSG_TYPE _type,
                                byte[] in_data,
                                out byte[] out_data,
                                out int lenth,
                                int timeout=50)
        {
            try
            {
                byte[] data = CreateSysPack(_type, in_data);
                this.WriteSysOneFrame(data);
                SocketPack pk = new SocketPack(new byte[0]);
                bool res = this.ReadOneSocketPack(out pk, true,timeout);
                if (pk.count == pk.CountToRandom((byte)0) && 
                    (SYS_MSG_TYPE)(((ushort)pk.data[0]<<SocketPack.FIRST_BYTE)| ((ushort)pk.data[1] << SocketPack.SECOND_BYTE)) == _type)
                {
                    out_data = pk.data;
                    lenth = pk.lenth;
                    return true;
                }
                else {
                    out_data = new byte[0];
                    lenth = 0;
                    return false;
                }
            }
            catch {
                out_data = new byte[0];
                lenth = 0;
                return false;
            }
        }
        private bool SendHeartBreak()
        {
            byte[] rand_byte = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            byte[] buffer = new byte[100];
            int lenth = 0;
            return this.SendSysMsg(SYS_MSG_TYPE.HEART_BREAK, rand_byte, out buffer, out lenth);
        }
        private bool SetCount(byte count)
        {
            byte[] write_byte = { count};
            byte[] buffer = new byte[100];
            int lenth = 0;
            bool res = this.SendSysMsg(SYS_MSG_TYPE.SET_COUNT, write_byte, out buffer, out lenth);
            for (int i = 0; i < lenth; i++)
            {
                Console.Write("0x"+buffer[i].ToString("X") + " ");
            }
            if (res) {
                this.dpack.target_count = count;
            }
            return res;
        }
        private void ClientSysHandle() {

        }
        public bool Connect() {
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this.sock.Connect(ipe);
                
                ThreadStart HandleRevDataThread = new ThreadStart(this.HandleRevData);
                this.RevThread = new Thread(HandleRevDataThread);
                this.RevThread.IsBackground = true;
                this.RevThread.Start();//启动新线程
                ThreadStart ClientSysHandleThread = new ThreadStart(this.ClientSysHandle);
                this.ClientSysThread = new Thread(ClientSysHandleThread);
                this.ClientSysThread.IsBackground = true;
                this.ClientSysThread.Start();//启动新线程
                for (int i = 0; i < 10; i++) {
                    SendHeartBreak();
                }
                SetCount(10);
                this.isConnect = true;
                return true;
            }
            catch(SocketException) {
                this.Close();
                this.isConnect = false;
                return false;
            }
        }
        //public bool WriteOneFrame(byte[] data) {
        //    // 
        //    SocketPack pk = new SocketPack(data);
        //    foreach (byte i in pk.StructToBytes())
        //    {
        //        Console.Write("0x" + i.ToString("x") + " ");
        //    }
        //    try
        //    {
        //        this.sock.Send(pk.StructToBytes());
        //        return true;
        //    }
        //    catch {
        //        return false;
        //    }
        //}
        private bool WriteSysOneFrame(byte[] data)
        {
            // 写入一帧系统数据
            SocketPack pk = new SocketPack(data,0);
            try
            {
                this.sock.Send(pk.StructToBytes());
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public bool ReadOneFrame(byte[] data, out int lenth,bool wait=false,int timeout=10)
        //{
        //    // 读取一帧数据
        //    bool res;
        //    SocketPack sk = new SocketPack(new byte[0]);
        //    if (wait == true)
        //    {
        //        int cnt = 0;
        //        do {
        //            res = this.dpack.GetPack(out sk);
        //            Thread.Sleep(1);
        //            cnt++;
        //            if (cnt == timeout) {
        //                throw (new System.IO.IOException("ReadOneFrame() timeout"));
        //            }
        //        } while (res!=true);
        //    }
        //    else {
        //        res = this.dpack.GetPack(out sk);
        //    }
            
        //    for (int i=0; i < sk.data.Length; i++) {
        //        data[i] = sk.data[i];
        //    }
        //    lenth = sk.data.Length;
        //    return res;
        //}

        public bool ReadOneSocketPack(out SocketPack pk, bool wait = false, int timeout = 10)
        {
            // 读取一个数据包
            bool res;
            if (wait == true)
            {
                int cnt = 0;
                do
                {
                    res = this.dpack.GetPack(out pk);
                    Thread.Sleep(1);
                    cnt++;
                    if (cnt == timeout)
                    {
                        throw (new System.IO.IOException("ReadOneFrame() timeout"));
                    }
                } while (res != true);
            }
            else
            {
                res = this.dpack.GetPack(out pk);
            }
            return res;
        }

        public void Close() {
            this.sock.Close();
        }
    }

    class Program
    {
        static void test_socketpack()
        {
            SocketPack apack = new SocketPack(Encoding.Default.GetBytes("abcd"));
            SocketPack bpack = new SocketPack(Encoding.Default.GetBytes("12345"));
            SocketPack cpack = new SocketPack(Encoding.Default.GetBytes("zzxxcc"));
            apack.Print();
            bpack.Print();
            cpack.Print();
            ManagerSocketPack dp = new ManagerSocketPack();

            dp.HandleBytesData(apack.StructToBytes(), apack.StructToBytes().Length);
            dp.HandleBytesData(Encoding.UTF8.GetBytes("123"), Encoding.UTF8.GetBytes("123").Length);
            dp.HandleBytesData(bpack.StructToBytes(),bpack.StructToBytes().Length);
            dp.HandleBytesData(Encoding.UTF8.GetBytes("123"), Encoding.UTF8.GetBytes("123").Length);
            dp.HandleBytesData(cpack.StructToBytes(), cpack.StructToBytes().Length);
            SocketPack p = new SocketPack(new byte[0]);
            while (true)
            {
                bool s = dp.GetPack(out p);
                if (s)
                {
                    Console.WriteLine("--------------------------------------");
                    p.Print();
                }
                else
                {
                    Console.WriteLine("-------------数据包获取完毕----------------");
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            TcpClient tc = new TcpClient("127.0.0.1", 8000);
            if (tc.Connect())
            {
                Thread.Sleep(10);
                //for (int i = 0; i < 100; i++) {
                //    tc.WriteOneFrame(Encoding.UTF8.GetBytes("asddfagdsag"));
                //    byte[] buffer = new byte[200];
                //    int len = 0;
                //    bool res = tc.ReadOneFrame(buffer,out len,true);
                //    string recStr = Encoding.ASCII.GetString(buffer, 0, len);
                //    Console.Write(i.ToString()+":");
                //    Console.WriteLine(recStr);
                //}
            }
            else {
                Console.WriteLine("connect fail!!!");
                tc.Close();
            }
        }
    }
}
