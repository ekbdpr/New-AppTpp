   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:NewAppTpp.ConvertersNewAppTpp.Converters  5      9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5
      $    NewAppTpp.MVVM.View.KelolaData   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1cv"clr-namespace:NewAppTpp.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5
        � 	Resources  5         CenterGridHeaderStyle  DataGridColumnHeaderStyle �� BasedOn �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value4   .NewAppTpp.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter��%�   &	        7  &	 {     �� 5   
   8   "  5      .�5      ��  � ��$ Center=�6;   �� 5       � ��$
 Bold"�6+   5        5   
   6'   5       � DataContext 5      2  ,NewAppTpp.MVVM.ViewModel.KelolaDataViewModel  5   
   .��5!      '  !HandyControl.Controls.SimplePanel  .+�5#   
   ,  &HandyControl.Controls.SimpleStackPanel  $��20q�6   .+�5&      &   HandyControl.Controls.SimpleText  $��Center=�5'        LargeFontSize   FontSize# �  5(        PrimaryTextBrush   
Foreground# �  5)         Text$ Kelola Data Pegawai��5*      #  HandyControl.Controls.Divider  5.        DarkPrimaryBrush	   
LineStroke#	 �  6     52      
   Orientation$
 
Horizontal=�6"   .+�55      $  HandyControl.Controls.ComboBox  $	��150��56      '	  !HandyControl.Controls.InfoElement 	  Placeholder$ Pilih Bulan��57      (
  "HandyControl.Controls.TitleElement 
  Title$ Bulan��58       � Cursor$
 Hand��59       �� SelectedValuePath$ Content��5;       �� SelectedValue 5:      �� *Bulan+ �� Mode$ TwoWay=�.@�5=      �� $��Januari��6#   �� 5>      $��Februari��6#   �� 5?      $��Maret��6#   �� 5@      $��April��6#   �� 5A      $	��Mei��6#   �� 5B      $
��Juni��6#   �� 5C      $
��Juli��6#   �� 5D      $��Agustus��6#   �� 5E      $��	September��6#   �� 5F      $��Oktober��6#   �� 5G      $��November��6#   �� 5H      $��Desember��6#   5J        5N      $	��150��5O      $��20,0,0,0q�5P      $ Pilih Tahun��5Q      $ Tahun��5R      $
 Hand��5S      $ Content��5U       5T      �� *Tahun+$ TwoWay=�.@�5W      �� $
��2024��6#   �� 5X      $
��2025��6#   �� 5Y      $
��2026��6#   �� 5Z      $
��2027��6#   �� 5[      $
��2028��6#   �� 5\      $
��2029��6#   �� 5]      $
��2030��6#   5_      �� 5c      $	��100��5d      $��30��5e      $��20,0,0,0q�5f      $��Bottom=�5g      )  #HandyControl.Controls.BorderElement   CornerRadius$ 15��5h      $��Submit��5j      $
 Hand��5k        ButtonPrimary#���  5l      ��5i      �� *SubmitSearchCommand+5l      5o        5r      $��Right=�6"   $
 
Horizontal=�6>   .+�5u      %  HandyControl.Controls.SearchBar  $	��380��5v      $	��0,5q�5w      $ 	Cari User��5x      $ Pencarian : ��5y       
  TitlePlacement$
 Left=�5z       
  
TitleWidth$ 80 �5{         
IsRealTime$
 True��5}        SearchBarExtend#���  5~         Command 5|      �� *SearchPegawaiCommand+��5      �� *
SearchText+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�&   HandyControl.Controls.Pagination  5�      $	��0,5q�5�      $
 Hand��5�         DataCountPerPage$ 10��5�         MaxPageInterval$ 2��5�         MaxPageCount 5�      �� *MaxPageCount+   	PageIndex 5�      �� *	PageIndex+$ TwoWay=�5�      )  #HandyControl.Controls.LoadingCircle  5�      $	��100q�6   y�6,   �� *	IsLoading+ �� 	Converter# [ &   System.Windows.Controls.DataGrid  5�      $��0,5,0,0q�5�         AutoGenerateColumns$ False��5�         CanUserAddRows$ False��5�         CanUserDeleteRows$ False��5�         CanUserReorderColumns$ False��5�          CanUserResizeRows$  False��5�      !   CanUserSortColumns$! False��5�      "   ColumnHeaderStyle#" �   5�      #   HeadersVisibility$# Column=�5�      $   
IsReadOnly$
$ True��5�      %   RowHeaderWidth$% 60Q�5�      ��5�      �� *PegawaiModelCollection+& �� SelectedItem& 5�      �� *SelectedPegawai+y�5�      �� *IsDataGridVisible+# [ '   Columns' 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn(   Width5  /System.Windows.Controls.DataGridLengthConverter$( 1* 5�      )   Header$	) NIP��5�      1  +System.Windows.Controls.DataGridBoundColumn*   Binding* 5�      �� *Nip+5�        5�      $( 1* 5�      $
) Nama��5�      * 5�      �� *Nama+5�        5�      $( 1* 5�      $) Kode Satker��5�      * 5�      �� *
KdSatker+5�        5�      $( 1* 5�      $) No.Rekening��5�      * 5�      �� *Norek+5�        5�      $
( 0.5* 5�      $) Kode
Pkt��5�      * 5�      �� *	KdPangkat+5�        5�      $
( 0.7* 5�      $
) Piwp��5�      * 5�      �� *Piwp++ �� StringFormat$+ Rp{0:N0}��5�        5�      $( 1* 5�      $) 	Nama SKPD��5�      * 5�      �� *NmSkpd+5�        5�      $( 1* 5�      $) Pagu TPP
Beban Kerja��5�      * 5�      �� *	PaguTppBk+$+ Rp{0:N0}��5�        5�      $( 1* 5�      $) Pagu TPP
Kondisi Kerja��5�      * 5�      �� *	PaguTppKk+$+ Rp{0:N0}��5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�      $( 1* 6-   $) Modifikasi
Data��68   ,   CellTemplate, 5�      �� 5�      .R�5�   "     $��Center=�66   $
 
Horizontal=�6S   .+�5�   &   �� $ 10��5�   )   $	��✎��5�   )   $
 Hand��5�   )     ButtonWarning#���  5�   )   ��5�   )   �� - �� RelativeSource- �� . �� AncestorType". >�/ �� Mode$/ FindAncestor=�0 �� Path$$0 DataContext.EditPegawaiCommand�5�   )   �� 5�   &   $��10,0,0,0q�5�   )   $ 10��5�   )   $
��🗑��5�   )   $
 Hand��5�   )    	 ButtonDanger#��� 	 5�   )   ��5�   )   �� - �� ". >�$/ FindAncestor=�$&0  DataContext.DeletePegawaiCommand�5�   )   5�   #   5�      5�      5�      5�      5�      5�      