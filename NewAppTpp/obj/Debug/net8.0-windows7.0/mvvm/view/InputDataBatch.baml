   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:NewAppTpp.ConvertersNewAppTpp.Converters  5      9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5
      (    "NewAppTpp.MVVM.View.InputDataBatch   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1cv"clr-namespace:NewAppTpp.Converters     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5
        � 	Resources  5      4   .NewAppTpp.Converters.BoolToVisibilityConverter   BoolToVisibilityConverter��%   &	          5   
   6'   5       � DataContext 5      6  0NewAppTpp.MVVM.ViewModel.InputDataBatchViewModel  5   
   .��5      '  !HandyControl.Controls.SimplePanel  .+�5   
   ,  &HandyControl.Controls.SimpleStackPanel  $��20q�6   .+�5      &   HandyControl.Controls.SimpleText  $��Center=�5        LargeFontSize   FontSize# �  5        PrimaryTextBrush   
Foreground# �  5         Text$ Input Data Batch��5       #  HandyControl.Controls.Divider  5$        DarkPrimaryBrush   
LineStroke# �  6     5(         Orientation$ 
Horizontal=�6"   .+�5+      $  HandyControl.Controls.ComboBox  $	��150��5,      '  !HandyControl.Controls.InfoElement   Placeholder$ Pilih Bulan��5-      (	  "HandyControl.Controls.TitleElement 	  Title$ Bulan��5.      	 � Cursor$
	 Hand��5/      
 �� SelectedValuePath$
 Content��51       �� SelectedValue 50      �� *Bulan+ �� Mode$ TwoWay=�.@�53      �� $��Januari��6#   �� 54      $��Februari��6#   �� 55      $��Maret��6#   �� 56      $��April��6#   �� 57      $	��Mei��6#   �� 58      $
��Juni��6#   �� 59      $
��Juli��6#   �� 5:      $��Agustus��6#   �� 5;      $��	September��6#   �� 5<      $��Oktober��6#   �� 5=      $��November��6#   �� 5>      $��Desember��6#   5@        5D      $	��150��5E      $��20,0,0,0q�5F      $ Pilih Tahun��5G      $ Tahun��5H      $
	 Hand��5I      $
 Content��5K       5J      �� *Tahun+$ TwoWay=�.@�5M      �� $
��2024��6#   �� 5N      $
��2025��6#   �� 5O      $
��2026��6#   �� 5P      $
��2027��6#   �� 5Q      $
��2028��6#   �� 5R      $
��2029��6#   �� 5S      $
��2030��6#   5U      5X        5\      $��0,20,0,0q�6"   $ 
Horizontal=�64   .+�5_      �� $	��120��5`      $��30��5a      )
  #HandyControl.Controls.BorderElement 
  CornerRadius$ 15��5b      $��
Pilih File��5d      $
	 Hand��5e      ��5c      �� *ChooseFileCommand+5e        5i      $��20,0,0,0q�5j      $��Center=�5k      # �  5l       5m      �� *
FileName+5p        5t      $��0,20,0,0q�6"   $ 
Horizontal=�64   .+�5w      �� $	��120��5x      $��30��5y      $ 15��5z      $��Import��5|      $
	 Hand��5}        ButtonPrimary#���  5~      ��5{      �� *ImportFileCommand+5~        5�      $��20,0,0,0q�6!   .+�5�      )  #HandyControl.Controls.LoadingCircle  $��30��6'   y�63   �� *	IsLoading+ �� 	Converter# [    5�      $��Center=�5�      $��Center=�5�        HeadFontSize# �  5�        SuccessBrush# �  5�      $	 ✅��5�      y�5�      �� *	IsSuccess+# [    5�      $��Center=�5�      $��Center=�5�      # �  5�         
FontWeight$ 	ExtraBold"�5�        DangerBrush# �  5�      $	 ❌��5�      y�5�      �� *	IsError+# [  5�      5�      5�      5�      5�      