   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5	      &     NewAppTpp.MVVM.View.LihatLaporan   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5	        � DataContext  5      4  .NewAppTpp.MVVM.ViewModel.LihatLaporanViewModel  5   
   .��5      '  !HandyControl.Controls.SimplePanel  .+�5   
   ,  &HandyControl.Controls.SimpleStackPanel  $��20q�6   .+�5      &   HandyControl.Controls.SimpleText  $��Center=�5         LargeFontSize   FontSize# �   5        PrimaryTextBrush   
Foreground# �  5         Text$ Lihat Laporan��5      #  HandyControl.Controls.Divider  5        DarkPrimaryBrush   
LineStroke# �  6     5#         Orientation$ 
Horizontal=�6"   .+�5&      $  HandyControl.Controls.ComboBox  $	��150��5'      '  !HandyControl.Controls.InfoElement   Placeholder$ Pilih Bulan��5(      (  "HandyControl.Controls.TitleElement   Title$ Bulan��5)       � Cursor$
 Hand��5*      	 �� SelectedValuePath$	 Content��5,      
 �� SelectedValue
 5+      �� *Bulan+ �� Mode$ TwoWay=�.@�5.      �� $��Januari��6#   �� 5/      $��Februari��6#   �� 50      $��Maret��6#   �� 51      $��April��6#   �� 52      $	��Mei��6#   �� 53      $
��Juni��6#   �� 54      $
��Juli��6#   �� 55      $��Agustus��6#   �� 56      $��	September��6#   �� 57      $��Oktober��6#   �� 58      $��November��6#   �� 59      $��Desember��6#   5;        5?      $	��150��5@      $��20,0,0,0q�5A      $ Pilih Tahun��5B      $ Tahun��5C      $
 Hand��5D      $	 Content��5F      
 5E      �� *Tahun+$ TwoWay=�.@�5H      �� $
��2024��6#   �� 5I      $
��2025��6#   �� 5J      $
��2026��6#   �� 5K      $
��2027��6#   �� 5L      $
��2028��6#   �� 5M      $
��2029��6#   �� 5N      $
��2030��6#   5P      5S        5W      $	��320��5X      $��0,20,0,0q�5Y      $
��Left=�5Z      $ Pilih Kepala Dinas��5[      $ Kepala Dinas��5\      $
 Hand��5]      $		 Nip��5`      ��5^      �� *PegawaiList+
 5_      �� *SelectedKepalaDinas+$ TwoWay=���5b      �� 5c      .R�5d      ��  �� Orientation$ 
Horizontal=�6%   .+�5e      �� ��6(   �� *Nip+�� 5f      $	�� - ��6(   �� 5g      ��6(   �� *Nama+5h      5i      5l        5p      $	��320��5q      $��0,20,0,0q�5r      $
��Left=�5s      $ Pilih Kasubag��5t      $ Kasubag Umum Kepegawaian��5u      $
 Hand��5v      $		 Nip��5y      ��5w      �� *PegawaiList+
 5x      �� *SelectedKasubag+$ TwoWay=���5{      �� 5|      .R�5}      �� $ 
Horizontal=�6%   .+�5~      �� ��6(   �� *Nip+�� 5      $	�� - ��6(   �� 5�      ��6(   �� *Nama+5�      5�      5�      �� 5�      $	��120��5�      $��30��5�      $
��0,20q�5�      $
��Left=�5�      )	  #HandyControl.Controls.BorderElement 	  CornerRadius$ 15��5�      $��Unduh��5�      $
 Hand��5�        ButtonPrimary#���  5�      ��5�      �� *DownloadPdfCommand+5�      5�      5�      5�      