   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5	      #    NewAppTpp.MVVM.View.Bendahara   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5	        � 	Resources  5         CenterGridHeaderStyle  DataGridColumnHeaderStyle �� BasedOn �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value��%�   &	        7  �� 5   
   8   "  5      .�5      ��  � ��$ Center=�6;   �� 5       � ��$
 Bold"�6+   5      5       � DataContext 5      1  +NewAppTpp.MVVM.ViewModel.BendaharaViewModel  5   
   .��5      '  !HandyControl.Controls.SimplePanel  .+�5    
   ,  &HandyControl.Controls.SimpleStackPanel  $��20q�6   .+�5#      &   HandyControl.Controls.SimpleText  $��Center=�5$        LargeFontSize   FontSize# �  5%        PrimaryTextBrush   
Foreground# �  5&         Text$ Bendahara Pengeluaran��5'      #  HandyControl.Controls.Divider  5+        DarkPrimaryBrush	   
LineStroke#	 �  6     5/      
   Orientation$
 
Horizontal=�6"   .+�52      $  HandyControl.Controls.ComboBox  $	��150��53      '  !HandyControl.Controls.InfoElement   Placeholder$ Pilih Bulan��54      (	  "HandyControl.Controls.TitleElement 	  Title$ Bulan��55       � Cursor$
 Hand��56       �� SelectedValuePath$ Content��58       �� SelectedValue 57      �� *Bulan+ �� Mode$ TwoWay=�.@�5:      �� $��Januari��6#   �� 5;      $��Februari��6#   �� 5<      $��Maret��6#   �� 5=      $��April��6#   �� 5>      $	��Mei��6#   �� 5?      $
��Juni��6#   �� 5@      $
��Juli��6#   �� 5A      $��Agustus��6#   �� 5B      $��	September��6#   �� 5C      $��Oktober��6#   �� 5D      $��November��6#   �� 5E      $��Desember��6#   5G        5K      $	��150��5L      $��20,0,0,0q�5M      $ Pilih Tahun��5N      $ Tahun��5O      $
 Hand��5P      $ Content��5R       5Q      �� *Tahun+$ TwoWay=�.@�5T      �� $
��2024��6#   �� 5U      $
��2025��6#   �� 5V      $
��2026��6#   �� 5W      $
��2027��6#   �� 5X      $
��2028��6#   �� 5Y      $
��2029��6#   �� 5Z      $
��2030��6#   5\      �� 5`      $	��100��5a      $��30��5b      $��20,0,0,0q�5c      $��Bottom=�5d      )
  #HandyControl.Controls.BorderElement 
  CornerRadius$ 15��5e      $��Submit��5g      $
 Hand��5h        ButtonPrimary#���  5i      ��5f      �� *SubmitSearchCommand+5i      5l        5o      $��Right=�6"   $
 
Horizontal=�6>   .+�5r      %  HandyControl.Controls.SearchBar  $	��380��5s      $	��0,5q�5t      $ 	Cari User��5u      $ Pencarian : ��5v       	  TitlePlacement$
 Left=�5w       	  
TitleWidth$ 80 �5x         
IsRealTime$
 True��5z        SearchBarExtend#���  5{         Command 5y      �� *SearchPegawaiCommand+��5|      �� *
SearchText+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�&   HandyControl.Controls.Pagination  5�      $��20,5,0,0q�5�      $
 Hand��5�         DataCountPerPage$ 10��5�         MaxPageInterval$ 2��5�         MaxPageCount 5�      �� *MaxPageCount+   	PageIndex 5�      �� *	PageIndex+$ TwoWay=�5�      &   System.Windows.Controls.DataGrid  5�      $��0,5,0,0q�5�         AutoGenerateColumns$ False��5�         CanUserAddRows$ False��5�         CanUserDeleteRows$ False��5�         CanUserReorderColumns$ False��5�         CanUserResizeRows$ False��5�          CanUserSortColumns$  False��5�      !   ColumnHeaderHeight$! 60Q�5�      "   ColumnHeaderStyle#" �   5�      #   HeadersVisibility$# Column=�5�      $   
IsReadOnly$
$ True��5�      %   RowHeaderWidth$% 60Q�5�      ��5�      �� *PegawaiModelCollection+& �� SelectedItem& 5�      �� *SelectedPegawai+'   Columns' 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn(   Width5  /System.Windows.Controls.DataGridLengthConverter$
( 1.5* 5�      )   Header$	) NIP��5�      1  +System.Windows.Controls.DataGridBoundColumn*   Binding* 5�      �� *Nip+5�        5�      $( 2* 5�      $
) Nama��5�      * 5�      �� *Nama+5�        5�      $
( 0.8* 5�      $) Kode
Pkt��5�      * 5�      �� *	KdPangkat+5�        5�      $
( 1.3* 5�      $) Pencapaian
Kinerja
(%)��5�      * 5�      �� *CapaiKinerja+5�        5�      $
( 1.3* 5�      $) Potongan
Kehadiran
(%)��5�      * 5�      �� *PotonganPercentKehadiran+5�        5�      $
( 1.5* 5�      $) 	Tpp Netto��5�      * 5�      �� *	Tpp_Netto++ �� StringFormat$+ Rp{0:N0}��5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�      $
( 1.3* 6-   $) Modifikasi
Data��6:   ,   CellTemplate, 5�      �� 5�      .R�5�   "     $��Center=�66   $
 
Horizontal=�6S   .+�5�   &   �� $ 10��5�   )   $	��✎��5�   )   $
 Hand��5�   )     ButtonWarning#���  5�   )   ��5�   )   �� - �� RelativeSource- �� . �� AncestorType". >�/ �� Mode$/ FindAncestor=�0 �� Path$&0  DataContext.EditBendaharaCommand�5�   )   5�   #   5�      5�      5�      5�      5�      5�      