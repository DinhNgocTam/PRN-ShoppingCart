using BusinessObject.Model;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SaleWPFApp
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private readonly IMemberRepository memberRepository;
        //private readonly Member member;
        public Register()
        {
            InitializeComponent();
            memberRepository = new MemberRepository();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Member member = new Member
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    City = txtCity.Text,
                    CompanyName = txtCompanyName.Text,
                    Country = txtCountry.Text
                };
                if (memberRepository.FindByEmail(member.Email) != null)
                {
                    MessageBox.Show("Email Already Exist !");
                    return;
                }
                if (member.Password.Equals(txtRePassword.Password))
                {
                    memberRepository.Add(member);
                    MessageBox.Show("Register Successfull !");
                    MainWindow main = new MainWindow();
                    this.Close();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Wrong Password");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
