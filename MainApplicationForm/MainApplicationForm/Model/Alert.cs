using System;

namespace MainApplicationForm.Model
{
    class Alert
    {
        public void generateWarnings()
        {
            Console.Beep(1000, 900);
        }
    }
}
