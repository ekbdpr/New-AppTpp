   M S B A M L   `   `   `  ����D  @NewAppTpp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NewAppTpp.MVVM.ViewNewAppTpp.MVVM.View  5      C&clr-namespace:NewAppTpp.MVVM.ViewModelNewAppTpp.MVVM.ViewModel  5	      +    %NewAppTpp.MVVM.View.ConfirmationPopup   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      S OHandyControl, Version=3.5.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      -local!clr-namespace:NewAppTpp.MVVM.View  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      /vm&clr-namespace:NewAppTpp.MVVM.ViewModel  5	      $	��500��5         RegionBrush#���   5        �� CornerRadius$  10��5        EffectShadow5 J� Effect# �  5      
 J� Uid 5
      �� *ConfirmationPopupUid+ � DataContext 5      9  3NewAppTpp.MVVM.ViewModel.ConfirmationPopupViewModel  5   
   ._�5      ,  &HandyControl.Controls.SimpleStackPanel  $��20,40q�6   .+�5   
   &   HandyControl.Controls.SimpleText  -   5          Name$ ConfirmationMessage��5        LargeFontSize   FontSize# �  5        PrimaryTextBrush   
Foreground# �  5         TextAlignment$ Center=�5         TextWrapping$
 Wrap=�5        HandyControl.Controls.Row  5   
   $��0,40,0,0q�6   .+�5         HandyControl.Controls.Col  	   Span$	 12��6   .��5"      �� $	��100��5#      $��30��5$      )  #HandyControl.Controls.BorderElement
   CornerRadius$
 15��5%      $��Ya��5'       � Cursor$
 Hand��5(       �� 	IsDefault$
 True��5)        ButtonPrimary#���  5*      ��5&      �� *YesButtonCommand+5*      5,        5.      $	 12��6   .��50      �� $	��100��51      $��30��52      $
 15��53      $��Tidak��55      $
 Hand��56      $
 True��57        ButtonDanger#���  58      ��54      �� *NoButtonCommand+58      5:      5<      5>      5@      