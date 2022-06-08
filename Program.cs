using SCADA_ecl.Classes;
using System;

namespace SCADA_ecl
{
    class Program
    {
        static void Main(string[] args)
        {
            Device device = new Device();
            device.StatusForArm();
            
            Console.WriteLine(device.statusSet._status);
            Console.WriteLine(device.Status);
        }
    }
}
