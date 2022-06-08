using System;
using System.Linq;

public class ChmiPostersStatuses
{
    public bool poster_work_line { get; private set; }
    public bool poster_work_peoples { get; private set; }
    public bool poster_work_under_voltage { get; private set; }
    public bool poster_ground { get; private set; }

    public void setPoster(bool? setPoster, uint? posterNumber)
    {
        if (setPoster == true)
        {
            if ((posterNumber & 0x1) == 0x1)
                poster_work_line = true;
            if ((posterNumber & 0x2) == 0x2)
                poster_work_peoples = true;
            if ((posterNumber & 0x4) == 0x4)
                poster_work_under_voltage = true;
            if ((posterNumber & 0x8) == 0x8)
                poster_ground = true;
        }
    }

    public void resetPoster(bool? resetPoster, uint? posterNumber)
    {
        if (resetPoster == true)
        {
            if ((posterNumber & 0x1) == 0x1)
                poster_work_line = false;
            if ((posterNumber & 0x2) == 0x2)
                poster_work_peoples = false;
            if ((posterNumber & 0x4) == 0x4)
                poster_work_under_voltage = false;
            if ((posterNumber & 0x8) == 0x8)
                poster_ground = false;
        }
    }
}