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
        private MotionControlType _mct = MotionControlType.Velocity;
        [CustomSortedCategoryAttribute("Motion Config", 1, 6), DisplayName("MotionControlType"), PropertyOrder(0)]
        public MotionControlType MCT { get { return _mct; } set { _mct = value; } }

        public enum TorqueControlType
        {
            Voltage = 1,
            DCCurrent,
            FOCCurrent
        }
        private TorqueControlType _tct = TorqueControlType.Voltage;
        [CustomSortedCategoryAttribute("Motion Config", 1, 6), DisplayName("TorqueControlType"), PropertyOrder(1)]
        public TorqueControlType TCT { get { return _tct; } set { _tct = value; } }

        [CustomSortedCategoryAttribute("Motion Config", 1, 6), PropertyOrder(2)]
        public int MotionDownsample { get; set; }

        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public int VelocityProportionalGain { get; set; }

        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public int VelocityIntegralGain { get; set; }
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public int VelocityDerivativeGain { get; set; }
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public int VelocityOutputRamp { get; set; }
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public int VelocityOutputLitmit { get; set; }
        [CustomSortedCategoryAttribute("Velocity PID", 2, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public int VelocityLowPassFilter { get; set; }


        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public int AngleProportionalGain { get; set; }

        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public int AngleIntegralGain { get; set; }
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public int AngleDerivativeGain { get; set; }
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public int AngleOutputRamp { get; set; }
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public int AngleOutputLitmit { get; set; }
        [CustomSortedCategoryAttribute("Angle PID", 3, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public int AngleLowPassFilter { get; set; }


        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public int CurrentqProportionalGain { get; set; }

        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public int CurrentqIntegralGain { get; set; }
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public int CurrentqDerivativeGain { get; set; }
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public int CurrentqOutputRamp { get; set; }
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public int CurrentqOutputLitmit { get; set; }
        [CustomSortedCategoryAttribute("Current q PID", 4, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public int CurrentqLowPassFilter { get; set; }


        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("ProportionalGain"), PropertyOrder(0)]
        public int CurrentdProportionalGain { get; set; }

        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("IntegralGain"), PropertyOrder(1)]
        public int CurrentdIntegralGain { get; set; }
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("DerivativeGain"), PropertyOrder(2)]
        public int CurrentdDerivativeGain { get; set; }
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("OutputRamp"), PropertyOrder(3)]
        public int CurrentdOutputRamp { get; set; }
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("OutputLitmit"), PropertyOrder(4)]
        public int CurrentdOutputLitmit { get; set; }
        [CustomSortedCategoryAttribute("Current d PID", 5, 6), DisplayName("LowPassFilter"), PropertyOrder(5)]
        public int CurrentdLowPassFilter { get; set; }


        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(0)]
        public int VelocityLimit { get; set; }
        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(1)]
        public int VoltageLimit { get; set; }
        [CustomSortedCategoryAttribute("Limit", 6, 6), PropertyOrder(2)]
        public int CurrentLimit { get; set; }


        [CustomSortedCategoryAttribute("SensorConfig", 7, 6), PropertyOrder(0)]
        public int ZeroAngleOffset { get; set; }
        [CustomSortedCategoryAttribute("SensorConfig", 7, 6), PropertyOrder(1)]
        public int ElectricalZeroOffset { get; set; }


        [CustomSortedCategoryAttribute("GeneralResistance", 8, 6), PropertyOrder(0)]
        public int PhaseResistance { get; set; }
        [CustomSortedCategoryAttribute("GeneralResistance", 8, 6), PropertyOrder(1)]
        public int MotorStatus { get; set; }
    }
}
