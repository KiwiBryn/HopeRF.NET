﻿namespace RFMLib.Configuration.FskOok
{
    public class RFM9XFskOokFrequencyConfig
    {
        private readonly TransceiverRegistry frequiencyBank1;
        private readonly TransceiverRegistry frequiencyBank2;
        private readonly TransceiverRegistry frequiencyBank3;

        private readonly TransceiverRegistry preambleMsb;
        private readonly TransceiverRegistry preambleLsb;

        private readonly TransceiverRegistry powerRegistry;
        private readonly TransceiverRegistry paDacRegistry;

        private readonly TransceiverRegistry regModemConfig1;
        private readonly TransceiverRegistry regModemConfig2;
        private readonly TransceiverRegistry regModemConfig3;

        public RFM9XFskOokFrequencyConfig(ITransceiverSpiConnection connection)
        {
            this.frequiencyBank1 = new TransceiverRegistry(connection, 0x06);
            this.frequiencyBank2 = new TransceiverRegistry(connection, 0x07);
            this.frequiencyBank3 = new TransceiverRegistry(connection, 0x08);

            this.powerRegistry = new TransceiverRegistry(connection, 0x09);
            this.paDacRegistry = new TransceiverRegistry(connection, 0x4D);

            this.preambleMsb = new TransceiverRegistry(connection, 0x25);
            this.preambleLsb = new TransceiverRegistry(connection, 0x26);
        }

        public bool PaSelect
        {
            get
            {
                return this.powerRegistry.GetBit(7);
            }
            set
            {
                this.powerRegistry.SetBit(7, value);
            }
        }

        public int Power
        {
            get
            {
                return (this.powerRegistry.Value & 0x0F); // 00001111
            }
            set
            {
                this.powerRegistry.Value = (byte)((this.powerRegistry.Value & 0xF0) | (byte)value); // 11110000
            }
        }

        public int MaxPower
        {
            get
            {
                return (this.powerRegistry.Value & 0x70) >> 4; // 01110000
            }
            set
            {
                this.powerRegistry.Value = (byte)((this.powerRegistry.Value & 0x8F) | (byte)(value << 4)); // 10001111
            }
        }


        public int PaDac
        {
            get
            {
                return (this.paDacRegistry.Value & 0x07); // 00000111
            }
            set
            {
                this.paDacRegistry.Value = (byte)((this.paDacRegistry.Value & 0xF8) | (byte)value); // 11111000
            }
        }

        public long Frequency
        {
            get
            {
                long frf = 0;

                frf |= this.frequiencyBank1.Value << 16;
                frf |= this.frequiencyBank2.Value << 8;
                frf |= this.frequiencyBank3.Value << 0;

                return ((frf * 32000000) >> 19);
            }
            set
            {
                long l = (value);
                long frf = (l << 19) / 32000000;

                this.frequiencyBank1.Value = (byte)(frf >> 16);
                this.frequiencyBank2.Value = (byte)(frf >> 8);
                this.frequiencyBank3.Value = (byte)(frf >> 0);
            }
        }


        // lazy temp code
        public byte PreambleMsb
        {
            get
            {
                return this.preambleMsb.Value;
            }
            set
            {
                this.preambleMsb.Value = value;
            }
        }

        // lazy temp code
        public byte PreambleLsb
        {
            get
            {
                return this.preambleLsb.Value;
            }
            set
            {
                this.preambleLsb.Value = value;
            }
        }

        public void Read()
        {
            this.frequiencyBank1.Read();
            this.frequiencyBank2.Read();
            this.frequiencyBank3.Read();

            this.powerRegistry.Read();
            this.paDacRegistry.Read();

            this.regModemConfig1.Read();
            this.regModemConfig2.Read();
            this.regModemConfig3.Read();

            this.preambleMsb.Read();
            this.preambleLsb.Read();
        }

        public void Write()
        {
            this.frequiencyBank1.Write();
            this.frequiencyBank2.Write();
            this.frequiencyBank3.Write();

            this.powerRegistry.Write();
            this.paDacRegistry.Write();

            this.regModemConfig1.Write();
            this.regModemConfig2.Write();
            this.regModemConfig3.Write();

            this.preambleMsb.Write();
            this.preambleLsb.Write();
        }
    }
}