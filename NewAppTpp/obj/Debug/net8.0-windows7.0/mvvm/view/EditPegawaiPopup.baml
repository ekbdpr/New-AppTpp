   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5	      *    $NewAppTpp.MVVM.View.EditPegawaiPopup   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5	      $	��900��5
         RegionBrush#���   5        �� CornerRadius$  10��5        EffectShadow5 J� Effect# �  5       � DataContext 5      8  2NewAppTpp.MVVM.ViewModel.EditPegawaiPopupViewModel  5   
   ._�5      '  !HandyControl.Controls.SimplePanel  .+�5   
   ,  &HandyControl.Controls.SimpleStackPanel  $
��0,20q�6   .+�5      &   HandyControl.Controls.SimpleText    LargeFontSize   FontSize# �  5        PrimaryTextBrush   
Foreground# �  5         Text$ Edit Pegawai��5         TextAlignment$ Center=�5      #  HandyControl.Controls.Divider  5"      $	��850��6   %  HandyControl.Controls.FlexPanel  5&         JustifyContent$ SpaceBetween=�6   .+�5(        .+�5+           Orientation$ 
Horizontal=�6*   .+�5-        $	��100��5.      $��30,0,0,0q�5/      $��Center=�50      # �  51      $	 NIP��52        54      $	��280��55      $��30��56      $
��8,10q�57      '  !HandyControl.Controls.InfoElement	   Placeholder$	 Masukkan NIP��58      ${�False��59       5:      �� *Nip+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5<        5@      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5B        $	��100��5C      $��30,0,0,0q�5D      $��Center=�5E      # �  5F      $
 Nama��5G      #  HandyControl.Controls.TextBox  5I      $	��280��5J      $��30��5K      $	 Masukkan Nama��5L      ��5M      �� *Nama+$
 TwoWay=�$ PropertyChanged=�5O        5S      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5U        $	��100��5V      $��30,0,0,0q�5W      $��Center=�5X      # �  5Y      $ Kode Satker��5Z        5\      $	��280��5]      $��30��5^      $	 Masukkan Kode Satker��5_      ��5`      �� *
KdSatker+$
 TwoWay=�$ PropertyChanged=�5b        5f      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5h        $	��100��5i      $��30,0,0,0q�5j      $��Center=�5k      # �  5l      $ No Rekening��5m        5o      $	��280��5p      $��30��5q      $	 Masukkan No Rekening��5r      ��5s      �� *Norek+$
 TwoWay=�$ PropertyChanged=�5u        5y      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5{        $	��100��5|      $��30,0,0,0q�5}      $��Center=�5~      # �  5      $ Kode Pangkat��5�        5�      $	��280��5�      $��30��5�      $	 Masukkan Kode Pangkat��5�      ��5�      �� *	KdPangkat+$
 TwoWay=�$ PropertyChanged=�5�      5�        5�         Orientation$ Vertical=�6     5�      $��0,0,30,0q�6&   .+�5�        $ 
Horizontal=�6*   .+�5�        $	��100��5�      $��0,0,10,0q�5�      $��Center=�5�      # �  5�      $
 Piwp��5�        5�      $	��280��5�      $��30��5�      $	 Masukkan Piwp��5�      ��5�      �� *Piwp+$
 TwoWay=�$ PropertyChanged=�5�        5�      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5�        $	��100��5�      $��0,0,10,0q�5�      $��Center=�5�      # �  5�      $ 	Nama SKPD��5�        5�      $	��280��5�      $��75��5�      $	 Masukkan Nama SKPD��5�       �� TextWrapping$
 Wrap=�5�      ��5�      �� *NmSkpd+$
 TwoWay=�$ PropertyChanged=�5�      5�        5�      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5�        $	��100��5�      $��0,0,10,0q�5�      $��Center=�5�      # �  5�      $ Pagu TPP Beban Kerja��5�         TextWrapping$
 Wrap=�5�        5�      $	��280��5�      $��30��5�      $#	 Masukkan Pagu TPP Beban Kerja��5�      ��5�      �� *	PaguTppBk+$
 TwoWay=�$ PropertyChanged=�5�        5�      $��0,20,0,0q�6*   $ 
Horizontal=�6<   .+�5�        $	��100��5�      $��0,0,10,0q�5�      $��Center=�5�      # �  5�      $ Pagu TPP Kondisi Kerja��5�      $
 Wrap=�5�        5�      $	��280��5�      $��30��5�      $%	 Masukkan Pagu TPP Kondisi Kerja��5�      ��5�      �� *	PaguTppKk+$
 TwoWay=�$ PropertyChanged=�5�      5�      5�        5�      $	��850��6   �� 5�      $	��100��5�      $��40��5�      $
��Save��5�       � Cursor$
 Hand��5�        ButtonPrimary#���  5�      ��5�      �� *SaveCommand+5�      5�      �� 5�   
   $��22��5�      $��22��5�      $��0,4,4,0q�5�      $��0q�5�      $��Right=�5�      $	��Top=�5�        ErrorGeometry'	  !HandyControl.Controls.IconElement 	  Geometry# �  5�      $��hc:ControlCommands.Close��5�      $
 Hand��5�        DangerBrush#���  5�        
ButtonIcon#���  5      5     5     