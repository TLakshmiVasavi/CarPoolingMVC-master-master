using Models.Enums;
using System;
using System.Text.RegularExpressions;

namespace CarPooling
{
    class Reader
    {
        public static float ReadFloat(float min, float max)
        {
            float Value = float.Parse(ReadNumber());
            if (Value < min ||Value > max)
            {
                Console.WriteLine("Please enter a valid value");
                return ReadFloat();
            }
            else
            {
                return Value;
            }
        }

        public static DateTime ReadDateTime()
        {
            string value = Console.ReadLine();
            if (DateTime.TryParse(value, out DateTime dateTime))
            {
                if (DateTime.Compare(dateTime, DateTime.Now) > 0)
                {
                    return dateTime;
                }
            }
            Console.WriteLine("Please enter a valid date");
            return ReadDateTime();
        }

        public static DateTime ReadDate()
        {
            string value = Console.ReadLine();
            if (DateTime.TryParse(value, out DateTime dateTime))
            {
                if (DateTime.Compare(dateTime.Date, DateTime.Now.Date) >= 0)
                {
                    return dateTime.Date;
                }
            }
            Console.WriteLine("Please enter a valid date");
            return ReadDate();
        }

        public static TimeSpan ReadTime()
        {
            string value = Console.ReadLine();
            if (TimeSpan.TryParse(value, out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            return ReadTime();
        }

        public static VehicleType ReadVehicleType()
        {
            string value = Console.ReadLine();
            if (Enum.TryParse<VehicleType>(value, out VehicleType VehicleType))
            {
                return VehicleType;
            }
            return ReadVehicleType();
        }

        public static bool ReadBool()
        {
            string value = ReadString();
            if (value == "Y" || value=="y" )
            {
                return true;
            }
            if (value == "N" || value=="n" )
            {
                return false;
            }
            Console.WriteLine("Please enter Y or N");
            return ReadBool();
        }

        public static string ReadMobileNumber()
        {
            string number = ReadString();
            if (Regex.IsMatch(number, @"^[0-9]{10}$"))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Sorry,The Mobile Number is not valid");
                return ReadMobileNumber();
            }
        }

        public static string ReadMail()
        {
            string mail = Console.ReadLine();
            if (Regex.IsMatch(mail, @"^\w+\@\w+\.[a-zA-z]{2,3}$"))
            {
                return mail;
            }
            else
            {
                Console.WriteLine("Sorry,The mail id is not valid");
                return ReadMail();
            }
        }

        public static string ReadNumber()
        {
            string value = ReadString();
            if (Regex.IsMatch(value, @"^[0-9\.\-]*$"))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Please enter a number");
                return ReadNumber();
            }
        }

        public static Gender ReadGender()
        {
            string value = ReadString();
            if (Regex.IsMatch(value, @"(?i)((Fe)?male|1|2)"))
            {
                return Enum.Parse<Gender>(value);
            }
            else
            {
                Console.WriteLine("Sorry,The Gender is not valid ");
                return ReadGender();
            }
        }

        public static string ReadPassword()
        {
            string pwd = Console.ReadLine();
            if (Regex.IsMatch(pwd, @"^((?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9])).{6,}"))
            {
                return pwd;
            }
            else
            {
                Console.WriteLine("Sorry,The password must contain a digit,letter in uppercase,a special character and minimum 6 characters ");
                return ReadPassword();
            }
        }

        public static int ReadInt()
        {
            string val = ReadNumber();
            int value = int.Parse(val);
            if (value < 0)
            {
                Console.WriteLine("Please enter a positive value");
                return ReadInt();
            }
            else
            {
                return value;
            }
        }

        public static int ReadInt(int min, int max)
        {
            string val = ReadNumber();
            int value = int.Parse(val);
            if (value >= min && value <= max)
            {
                return value;
            }
            else
            {
                Console.WriteLine("Please enter a valid value");
                return ReadInt(min, max);
            }
        }

        public static string ReadString()
        {
            string value = Console.ReadLine();
            if (Regex.IsMatch(value, @"^[ ]*$"))
            {
                Console.WriteLine("The value cannot be empty \n Please enter a value ");
                return ReadString();
            }
            else
            {
                return value;
            }
        }

        public static float ReadFloat()
        {
            float Value = float.Parse(ReadNumber());
            if (Value < 0)
            {
                Console.WriteLine("Please enter a positive value");
                return ReadFloat();
            }
            else
            {
                return Value;
            }
        }
    }
}