using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum CSAFE : uint
    {
        GETSTATUS_CMD = 0x80,
        RESET_CMD = 0x81,
        GOIDLE_CMD = 0x82,
        GOHAVEID_CMD = 0x83,
        GOINUSE_CMD = 0x85,
        GOFINISHED_CMD = 0x86,
        GOREADY_CMD = 0x87,
        BADID_CMD = 0x88,
        GETVERSION_CMD = 0x91,
        GETID_CMD = 0x92,
        GETUNITS_CMD = 0x93,
        GETSERIAL_CMD = 0x94,
        GETLIST_CMD = 0x98,
        GETUTILIZATION_CMD = 0x99,
        GETMOTORCURRENT_CMD = 0x9A,
        GETODOMETER_CMD = 0x9B,
        GETERRORCODE_CMD = 0x9C,
        GETSERVICECODE_CMD = 0x9D,
        GETUSERCFG1_CMD = 0x9E,
        GETUSERCFG2_CMD = 0x9F,
        GETTWORK_CMD = 0xA0,
        GETHORIZONTAL_CMD = 0xA1,
        GETVERTICAL_CMD = 0xA2,
        GETCALORIES_CMD = 0xA3,
        GETPROGRAM_CMD = 0xA4,
        GETSPEED_CMD = 0xA5,
        GETPACE_CMD = 0xA6,
        GETCADENCE_CMD = 0xA7,
        GETGRADE_CMD = 0xA8,
        GETGEAR_CMD = 0xA9,
        GETUPLIST_CMD = 0xAA,
        GETUSERINFO_CMD = 0xAB,
        GETTORQUE_CMD = 0xAC,
        GETHRCUR_CMD = 0xB0,
        GETHRTZONE_CMD = 0xB2,
        GETMETS_CMD = 0xB3,
        GETPOWER_CMD = 0xB4,
        GETHRAVG_CMD = 0xB5,
        GETHRMAX_CMD = 0xB6,
        GETUSERDATA1_CMD = 0xBE,
        GETUSERDATA2_CMD = 0xBF,
        SETUSERCFG1_CMD = 0x1A,
        SETTWORK_CMD = 0x20,
        SETHORIZONTAL_CMD = 0x21,
        SETPROGRAM_CMD = 0x24,
        SETTARGETHR_CMD = 0x30,
        PM_GET_WORKDISTANCE = 0xA3,
        PM_GET_WORKTIME = 0xA0,
        PM_SET_SPLITDURATION = 0x05,
        PM_GET_FORCEPLOTDATA = 0x6B,
        PM_GET_DRAGFACTOR = 0xC1,
        PM_GET_STROKESTATE = 0xBF,
        UNITS_METER = 0x24
    }
}
