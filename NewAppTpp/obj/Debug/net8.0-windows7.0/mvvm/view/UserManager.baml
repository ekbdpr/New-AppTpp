   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5	      %    NewAppTpp.MVVM.View.UserManager   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5	        � DataContext  5      3  -NewAppTpp.MVVM.ViewModel.UserManagerViewModel  5   
    � 	Resources 5         CenterGridHeaderStyle  DataGridColumnHeaderStyle �� BasedOn �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value��%�   &	        7  �� 5   
   8   "  5      .�5      ��  � ��$ Center=�6;   �� 5       � ��$
 Bold"�6+   5      5      .��5      '  !HandyControl.Controls.SimplePanel  .+�5   
   ,  &HandyControl.Controls.SimpleStackPanel  $��20q�6   .+�5!      &   HandyControl.Controls.SimpleText  $��Center=�5"        LargeFontSize   FontSize# �  5#        PrimaryTextBrush   
Foreground# �  5$         Text$ User Manager��5%      #  HandyControl.Controls.Divider  5)        DarkPrimaryBrush	   
LineStroke#	 �  6   �� 5-      $	��120��5.      $��30��5/      $
��Left=�50      )  #HandyControl.Controls.BorderElement
   CornerRadius$
 10��51      $��Tambah User��53       � Cursor$
 Hand��54        ButtonPrimary#���  55      ��52      �� *AddUserCommand+55        58      $	��0,5q�59      $��Right=�5:         Orientation$ 
Horizontal=�5;      .+�5>      %  HandyControl.Controls.SearchBar  $	��380��5?      '	  !HandyControl.Controls.InfoElement 	  Placeholder$ 	Cari User��5@      (
  "HandyControl.Controls.TitleElement 
  Title$ Pencarian : ��5A       
  TitlePlacement$
 Left=�5B       
  
TitleWidth$ 80 �5C         
IsRealTime$
 True��5E        SearchBarExtend#���  5F         Command 5D      �� *SearchUserCommand+��5G      �� *
SearchText+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�&   HandyControl.Controls.Pagination  5K      $	��0,5q�5L      $
 Hand��5M         DataCountPerPage$ 10��5N         MaxPageInterval$ 2��5P         MaxPageCount 5O      �� *MaxPageCount+   	PageIndex 5Q      �� *	PageIndex+$ TwoWay=�5T      &   System.Windows.Controls.DataGrid  5W      $��0,20,0,0q�5X         AutoGenerateColumns$ False��5Y         CanUserAddRows$ False��5Z         CanUserDeleteRows$ False��5[         CanUserReorderColumns$ False��5\         CanUserResizeColumns$ False��5]         CanUserResizeRows$ False��5^         CanUserSortColumns$ False��5_          ColumnHeaderStyle#  �   5`      !   HeadersVisibility$! Column=�5a      "   
IsReadOnly$
" True��5b      #   RowHeaderWidth$# 60Q�5d      ��5c      �� *UserAccessModelCollection+$ �� SelectedItem$ 5e      �� *SelectedUser+%   Columns% 5g      0  *System.Windows.Controls.DataGridTextColumn  5i      ,  &System.Windows.Controls.DataGridColumn&   Width5  /System.Windows.Controls.DataGridLengthConverter$& 1* 5j      '   Header$	' NIP��5l      1  +System.Windows.Controls.DataGridBoundColumn(   Binding( 5k      �� *Nip+5l        5n      $& 1* 5o      $
' Nama��5q      ( 5p      �� *Nama+5q        5s      $& 1* 5t      $' Jabatan��5v      ( 5u      �� *	Jabatan+5v        5x      $& 1* 5y      $' Username��5{      ( 5z      �� *
Username+5{        5}      $& 1* 5~      $' 	Privilege��5�      ( 5      �� *	Privilege+5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�      $& 1* 6-   $' Modifikasi Data��68   )   CellTemplate) 5�      �� 5�      .R�5�   "     $��Center=�66   $ 
Horizontal=�6S   .+�5�   &   �� $
 10��5�   )   $	��✎��5�   )   $
 Hand��5�   )     ButtonWarning#���  5�   )   ��5�   )   �� * �� RelativeSource* �� + �� AncestorType"+ >�, �� Mode$, FindAncestor=�- �� Path$!- DataContext.EditUserCommand�5�   )   �� 5�   &   $��10,0,0,0q�5�   )   $
 10��5�   )   $
��🗑��5�   )   $
 Hand��5�   )     ButtonDanger#���  5�   )   ��5�   )   �� * �� "+ >�$, FindAncestor=�$#- DataContext.DeleteUserCommand�5�   )   5�   #   5�      5�      5�      5�      5�      5�      