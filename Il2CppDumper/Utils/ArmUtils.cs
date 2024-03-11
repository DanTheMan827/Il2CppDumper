using System;
using System.Text;

namespace Il2CppDumper
{
    static class ArmUtils
    {
        public static uint DecodeMov(byte[] asm)
        {
            var low = (ushort)(asm[2] + ((asm[3] & 0x70) << 4) + ((asm[1] & 0x04) << 9) + ((asm[0] & 0x0f) << 12));
            var high = (ushort)(asm[6] + ((asm[7] & 0x70) << 4) + ((asm[5] & 0x04) << 9) + ((asm[4] & 0x0f) << 12));
            return (uint)((high << 16) + low);
        }

        public static ulong DecodeAdr(ulong pc, byte[] inst)
        {
            string bin = ByteArrayToBinaryString(inst);
            string uint64 = bin.Substring(8, 19) + bin.Substring(1, 2);
            uint64 = uint64.PadLeft(64, uint64[0]);
            return pc + Convert.ToUInt64(uint64, 2);
        }

        public static ulong DecodeAdrp(ulong pc, byte[] inst)
        {
            pc &= 0xFFFFFFFFFFFFF000;
            string bin = ByteArrayToBinaryString(inst);
            string uint64 = bin.Substring(8, 19) + bin.Substring(1, 2) + new string('0', 12);
            uint64 = uint64.PadLeft(64, uint64[0]);
            return pc + Convert.ToUInt64(uint64, 2);
        }

        private static string ByteArrayToBinaryString(byte[] bytes)
        {
            StringBuilder binaryStringBuilder = new StringBuilder(bytes.Length * 8); // Assuming 8 bits per byte
            foreach (byte b in bytes)
            {
                binaryStringBuilder.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return binaryStringBuilder.ToString();
        }

        public static ulong DecodeAdd(byte[] inst)
        {
            var bin = inst.HexToBin();
            var uint64 = Convert.ToUInt64(bin.Substring(10, 12), 2);
            if (bin[9] == '1')
                uint64 <<= 12;
            return uint64;
        }

        public static bool IsAdr(byte[] inst)
        {
            var bin = inst.HexToBin();
            return bin[0] == '0' && bin.Substring(3, 5) == "10000";
        }
    }
}
