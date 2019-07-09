using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Newtonsoft.Json;

namespace EyeAPI
{
    public class Station : NetworkNode
    {
        public StationDataStatus LatestDataStatus = StationDataStatus.Unknown;
        public StationStatus LatestStatus = StationStatus.Unknown;

        public Station(string _config)
        {
            _internal = new StationInternal();

            
        }

        StationInternal _internal;

        internal class StationInternal
        {
            public float coinctime;
            public float delay_check;
            public float delay_error;
            public float delay_screen;
            public int detnum;
            public float gps_altitude;
            public float gps_latitude;
            public float gps_longitude;
            public float mas_ch1_adc_gain;
            public float mas_ch1_adc_offset;
            public float mas_ch1_comp_gain;
            public float mas_ch1_comp_offset;
            public float mas_ch1_current;
            public int mas_ch1_gain_neg;
            public int mas_ch1_gain_pos;
            public float mas_ch1_inttime;
            public int mas_ch1_offset_neg;
            public int mas_ch1_offset_pos;
            public float mas_ch1_thres_high;
            public float mas_ch1_thres_low;
            public float mas_ch1_voltage;
            public float mas_ch2_adc_gain;
            public float mas_ch2_adc_offset;
            public float mas_ch2_comp_gain;
            public float mas_ch2_comp_offset;
            public float mas_ch2_current;
            public int mas_ch2_gain_neg;
            public int mas_ch2_gain_pos;
            public float mas_ch2_inttime;
            public int mas_ch2_offset_neg;
            public int mas_ch2_offset_pos;
            public float mas_ch2_thres_high;
            public float mas_ch2_thres_low;
            public float mas_ch2_voltage;
            public int mas_common_offset;
            public float mas_comp_thres_high;
            public float mas_comp_thres_low;
            public float mas_internal_voltage;
            public int mas_max_voltage;
            public bool mas_reset;
            public string mas_version;
            public float postcoinctime;
            public float precoinctime;
            public bool reduce_data;
            public float slv_ch1_adc_gain;
            public float slv_ch1_adc_offset;
            public float slv_ch1_comp_gain;
            public float slv_ch1_comp_offset;
            public float slv_ch1_current;
            public int slv_ch1_gain_neg;
            public int slv_ch1_gain_pos;
            public float slv_ch1_inttime;
            public int slv_ch1_offset_neg;
            public int slv_ch1_offset_pos;
            public float slv_ch1_thres_high;
            public float slv_ch1_thres_low;
            public float slv_ch1_voltage;
            public float slv_ch2_adc_gain;
            public float slv_ch2_adc_offset;
            public float slv_ch2_comp_gain;
            public float slv_ch2_comp_offset;
            public float slv_ch2_current;
            public int slv_ch2_gain_neg;
            public int slv_ch2_gain_pos;
            public float slv_ch2_inttime;
            public int slv_ch2_offset_neg;
            public int slv_ch2_offset_pos;
            public float slv_ch2_thres_high;
            public float slv_ch2_thres_low;
            public float slv_ch2_voltage;
            public float slv_common_offset;
            public float slv_comp_thres_high;
            public float slv_comp_thres_low;
            public int slv_internal_voltage;
            public float slv_max_voltage;
            public bool slv_reset;
            public string slv_version;
            public int spare_bytes;
            public bool startmode;
            public float summary;
            public DateTime timestamp;
            public bool trig_and_or;
            public int trig_external;
            public int trig_high_signals;
            public int trig_low_signals;
            public bool use_filter;
            public bool use_filter_threshold;
        }
    }

    public enum StationStatus
    {
        Up,
        Issue,
        Down,
        Unknown
    }
    public enum StationDataStatus
    {
        Up,
        Issue,
        Down,
        Unknown
    }

    
}
