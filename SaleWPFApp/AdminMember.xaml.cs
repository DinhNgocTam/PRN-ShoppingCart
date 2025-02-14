﻿using BusinessObject.Model;
using DataAccess.Model;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;


namespace SaleWPFApp
{
    /// <summary>
    /// Interaction logic for AdminMember.xaml
    /// </summary>
    public partial class AdminMember : Page
    {
        private readonly IMemberRepository memberRepository;
        public AdminMember(IMemberRepository _memberRepository)
        {
            InitializeComponent();
            this.memberRepository = _memberRepository;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dgData.ItemsSource = memberRepository.List().ToList();
        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            AdminMemberCreate adminMemberCreate = new AdminMemberCreate(this, null, memberRepository);
            adminMemberCreate.Show();
        }

        public void RefreshListView()
        {
            dgData.ItemsSource = memberRepository.List().ToList();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            var width = listView != null ? listView.ActualWidth - SystemParameters.VerticalScrollBarWidth : this.Width;

            var column1 = 0.1;
            var column2 = 0.2;
            var column3 = 0.2;
            var column4 = 0.2;
            var column5 = 0.1;
            var column6 = 0.2;

            if (gridView != null && width >= 0)
            {
                gridView.Columns[0].Width = width * column1;
                gridView.Columns[1].Width = width * column2;
                gridView.Columns[2].Width = width * column3;
                gridView.Columns[3].Width = width * column4;
                gridView.Columns[4].Width = width * column5;
                gridView.Columns[5].Width = width * column6;
            }
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            dgData.ItemsSource = memberRepository.List().ToList();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove member seledted?", "Remove member", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                List<Member> members = dgData.SelectedItems.Cast<Member>().ToList();
                members.ForEach(member => memberRepository.Delete(member));
                dgData.ItemsSource = memberRepository.List();
            }
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            int? id = !String.IsNullOrEmpty(searchById.Text) ? int.Parse(searchById.Text) : null;
            string email = searchByEmail.Text;
            string companyName = searchByCompanyName.Text;
            string city = searchByCity.Text;
            string country = searchByCountry.Text;

            MemberFilter memberFilter = new MemberFilter();
            memberFilter.MemberId = id;
            memberFilter.Email = email;
            memberFilter.CompanyName = companyName;
            memberFilter.City = city;
            memberFilter.Country = country;

            dgData.ItemsSource = memberRepository.FindAllBy(memberFilter);
        }
        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            int count = dgData.SelectedItems.Count;
            if (count > 0)
            {
                List<Member> members = dgData.SelectedItems.Cast<Member>().ToList();
                members.ForEach(member =>
                {
                    AdminMemberCreate adminMemberCreate = new AdminMemberCreate(this, member, memberRepository);
                    adminMemberCreate.Show();
                });
            }
            else
            {
                MessageBox.Show("Please select a member");
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row =
                (DataGridRow)dataGrid.ItemContainerGenerator
                .ContainerFromIndex(dgData.SelectedIndex);
            try
            {
                DataGridCell RowColumn =
               dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                if (string.IsNullOrEmpty(id))
                {
                    btnEdit.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                }
                else
                {


                    btnEdit.IsEnabled = true;
                    btnDelete.IsEnabled = true;

                }
            }
            catch (Exception ex)
            {
                dgData.ItemsSource = memberRepository.List().ToList();
            }
        }
    }
}