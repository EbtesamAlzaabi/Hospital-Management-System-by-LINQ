using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class Patient
    {
        public int patiendId {  get; set; }
        public string patientName { get; set; }
        public int patientAge { get; set; }
        public string patientGender { get; set; }
        public string patientPhone { get; set; }
        public string patientEmail { get; set; }
        public string patientBloodType { get; set; }

        public Patient(int id, string name, int age, string gender, string phone, string email, string bloodType)
        {
            patiendId = id;
            patientName = name;
            patientAge = age;
            patientGender = gender;
            patientPhone = phone;
            patientEmail = email;
            patientBloodType = bloodType;
        }

        public void printInfo()
        {
            Console.WriteLine($"ID: {patiendId}  |  Name: {patientName}  |  Age: {patientAge}" +
                                 $"  |  Gender: {patientGender}  |  Blood Type: {patientBloodType}" +
                                 $"  |  Phone: {patientPhone}  |  Email: {patientEmail}");

        }


    }
}
