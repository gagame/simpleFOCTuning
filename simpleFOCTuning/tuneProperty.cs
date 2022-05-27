using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleFOCTuning
{
    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("Name")]
    public class tuneProperty
    {
        public enum MotionControlType
        {
            Angle = 1,
            Velocity,
            Torque,
            VelocityOpenloop,
            AngleOpenloop
        }
        private MotionControlType? _mct = null;
        [CustomSortedCategoryAttribute("Motion Config", 1, 6), DisplayName("MotionControlType"), PropertyOrder(0)]
        public MotionControlType? MCT { get { return _mct; } set { _mct = value; } }

        public enum TorqueControlType
        {
            Voltage = 1,
            DCCurrent,
            FOCCurrent
        }
        private TorqueControlType? _tct = null;
        [CustomSortedCategoryAttribute("Motion Config", 1, 6), DisplayName("TorqueControlType"), PropertyOrder(1)]
        public TorqueControlType? TCT { get { return _tct; } set { _tct = value; } }

        private int? _md=null;
        [CustomSortedCategoryAttribute("Motion Config", 1, 6), PropertyOrder(2)]
        public int? MotionDownsample { get { return _md; } set { _md = value; } }

        private double? _vp=null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public double? VelocityProportionalGain { get { return _vp; } set { _vp = value; } }
        private double? _vi=null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public double? VelocityIntegralGain { get { return _vi; } set { _vi = value; } }
        private double? _vd = null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public double? VelocityDerivativeGain { get { return _vd; } set { _vd = value; } }
        private double? _vor=null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public double? VelocityOutputRamp { get { return _vor; } set { _vor = value; } }
        private double? _vol=null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public double? VelocityOutputLitmit { get { return _vol; } set { _vol = value; } }
        private double? _vlp=null;
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public double? VelocityLowPassFilter { get { return _vlp; } set { _vlp = value; } }

        private double? _ap = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public double? AngleProportionalGain { get { return _ap; } set { _ap = value; } }
        private double? _ai = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public double? AngleIntegralGain { get { return _ai; } set { _ai = value; } }
        private double? _ad = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public double? AngleDerivativeGain { get { return _ad; } set { _ad = value; } }
        private double? _aor = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public double? AngleOutputRamp { get { return _aor; } set { _aor = value; } }
        private double? _aol = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public double? AngleOutputLitmit { get { return _aol; } set { _aol = value; } }
        private double? _alp = null;
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public double? AngleLowPassFilter { get { return _alp; } set { _alp = value; } }

        private double? _cqp = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public double? CurrentqProportionalGain { get { return _cqp; } set { _cqp = value; } }
        private double? _cqi = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public double? CurrentqIntegralGain { get { return _cqi; } set { _cqi = value; } }
        private double? _cqd = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public double? CurrentqDerivativeGain { get { return _cqd; } set { _cqd = value; } }
        private double? _cqor = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public double? CurrentqOutputRamp { get { return _cqor; } set { _cqor = value; } }
        private double? _cqol = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public double? CurrentqOutputLitmit { get { return _cqol; } set { _cqol = value; } }
        private double? _cqlp = null;
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public double? CurrentqLowPassFilter { get { return _cqlp; } set { _cqlp = value; } }

        private double? _cdp = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public double? CurrentdProportionalGain { get { return _cdp; } set { _cdp = value; } }
        private double? _cdi = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public double? CurrentdIntegralGain { get { return _cdi; } set { _cdi = value; } }
        private double? _cdd = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public double? CurrentdDerivativeGain { get { return _cdd; } set { _cdd = value; } }
        private double? _cdor = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public double? CurrentdOutputRamp { get { return _cdor; } set { _cdor = value; } }
        private double? _cdol = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public double? CurrentdOutputLitmit { get { return _cdol; } set { _cdol = value; } }
        private double? _cdlp = null;
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public double? CurrentdLowPassFilter { get { return _cdlp; } set { _cdlp = value; } }


        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(0)]
        public double? VelocityLimit { get; set; }
        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(1)]
        public double? VoltageLimit { get; set; }
        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(2)]
        public double? CurrentLimit { get; set; }


        [CustomSortedCategoryAttribute("SensorConfig", 7, 6), PropertyOrder(0)]
        public double? ZeroAngleOffset { get; set; }
        [CustomSortedCategoryAttribute("SensorConfig", 7, 6), PropertyOrder(1)]
        public double? ElectricalZeroOffset { get; set; }


        [CustomSortedCategoryAttribute("GeneralResistance", 8, 6), PropertyOrder(0)]
        public double? PhaseResistance { get; set; }
        [CustomSortedCategoryAttribute("GeneralResistance", 8, 6), PropertyOrder(1)]
        public double? MotorStatus { get; set; }
    }
}
