using System;
using System.Diagnostics;
using Windows.Devices.Gpio;

namespace AP.NanoFrameWork.Vibration
{
    public class HDX
    {
        public event EventHandler InvokeEvent;

        private static GpioPinEdge _lastShockState = GpioPinEdge.FallingEdge;

        private System.Collections.ArrayList _shockHistory = new System.Collections.ArrayList();

        /// <summary>
        /// 
        /// </summary>
        public int Threshold { get; set; } = 15;
        public HDX()
        {

        }

        public void AnalyseSignal(GpioPinEdge edge)
        {
            if (_lastShockState != edge)
            {
                _lastShockState = edge;

                _shockHistory.Add(DateTime.UtcNow.Second);

                if (_shockHistory.Count > 0)
                {
                    int tmp = (int)_shockHistory[0];
                    int counter = 0;
                    bool removeOldHistory = false;
                    for (int i = 0; i < _shockHistory.Count; i++)
                    {

                        if (tmp == (int)_shockHistory[i])
                        {
                            counter++;
                        }
                        else
                        {
                            tmp = (int)_shockHistory[i];
                            counter = 0;
                            _shockHistory[i - 1] = -1; //Mark Index For Remove
                            removeOldHistory = true;
                        }

                        if (counter >= Threshold)
                        {
                            InvokeEvent?.Invoke(this, EventArgs.Empty);
                            _shockHistory.Clear();
                            break;
                        }

                        //IF Remove Mark Exist Then Remove items Untile Remove Mark
                        if (removeOldHistory)
                        {
                            removeOldHistory = false;

                            int j = 0;
                            int maxLen = _shockHistory.Count - 1;

                            while (true)
                            {
                                if ((int)_shockHistory[0] == -1)
                                {
                                    _shockHistory.RemoveAt(0);
                                    i = i - (j + 1);
                                    break;
                                }

                                _shockHistory.RemoveAt(0);
                                j++;

                                if (j > maxLen)
                                {
                                    break;
                                }
                            }
                        }


                    }

                }



                //  Debug.WriteLine("Detect!");
            }
        }

    }
}
