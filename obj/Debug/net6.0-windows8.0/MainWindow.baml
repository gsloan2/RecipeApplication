   M S B A M L   `   `   `  ����  
RecipesApp    RecipesApp.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Recipe Manager��5   	   $	��450��6    $	��900��6-    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5         ButtonStyle �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value  CategoryListBoxStyle  RecipeListBoxStyle  IconButtonStyle��%M  &	        &	 �    &	 �    &	 s    �� 5   
   " ��6$   .�5      ��  � �� �	#FF6DA34D6+   �� 5	       � ��
 �   �6+   �� 5
       � ��
 �   �6,   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 4q�6'   �� 5       � ��$	 8,4q�6(   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ 2q�60    5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� �	#FFF0F0F063   �� 5       � ��
 �   �63   5      5      �� 5   
   " ��6-   .�5      ��  � �� �	#FFF0F0F06+   �� 5       � ��
 ���� 6,   �� 5       � ��$ 10q�6'   �� 5       � ��$ 16(�6)   5      �� 5   
   " ��6+   .�5      ��  � �� �	#FFF0F0F06+   �� 5       � ��
 ���� 6,   �� 5        � ��$ 10q�6'   �� 5!       � ��$ 14(�6)   5"      �� 5#   
   " ��6(   .�5$      ��  � ��$ 50��6&   �� 5%       � ��$ 50��6'   �� 5&       � ��$ 5q�6'   �� 5'       � ��$ 5q�6(   5(      5)      .��5*      ]� .+�5+   
   c� -   5        	 c�Name$	 SearchPanel��5+      $��Stretch=�6'   $	��Top=�6E   $y�	Collapsed=�6Z   .@�5,      �� -   5        
 ��Name$
 SearchTextBox��5,      $=��7Enter ingredients to search(ex: potato, butter, pepper)��6-   $	��780��6l   $��5q�6x   6�   �� 5-      -   5         ��Name$ SearchButton��5-      $��Search��6+   #��[  6W    �� 	IsDefault$
 True��6|   5.      c� 5/   
   -   5        $	 HomePagePanel��5/      $	��Top=�6)   .@�50      �� -   5        $ AddCategoryButton��50      $��Add Category��60   #��[  6g   �� 51      -   5        $ EditCategoryButton��51      $��Edit Category��61   #��[  6j   �� 52      -   5        $ DeleteCategoryButton��52      $��Delete Category��63   #��[  6p   �� 53      �� 54      -   5        $ AddRecipeButton��54      $��
Add Recipe��6.   #��[  6a   �� 55      -	   5        $ ImportRecipeButton��55      $��Import Recipe��61   #��[  6j   �� 56      -
   5        $ EditRecipeButton��56      $��Edit Recipe��6/   #��[  6d   �� 57      -   5        $ ExportRecipeButton��57      $��Export Recipe��61   #��[  6j   �� 58      -   5        $ DeleteRecipeButton��58      $��Delete Recipe��61   #��[  6j   59      � 5:   
    � ColumnDefinitions 5;      �� 5<      $
��Auto �6#   �� 5=      $	��200 �6#   �� 5>      $��* �6#   .+�5B      �� $��0��6    �� Orientation$ Vertical=�6)   .+�5C      �� -   5        #��[ 5C      .��5D      �� $��resources/home.png��6   5E      �� 5F      -   5        #��[ 5F      .��5G      �� $��resources/search.png��6   5H      5I      �� 5L      -   5         ��Name$ CategoryList��5L      $��1��6,    �� SelectionMode$ Single=�6<    �� DisplayMemberPath$
 Name��6�   #��[ 6   �� SelectedItem 6�   �� *SelectedCategory+ �� Mode$ TwoWay=�6  �� 5O      -   5        $ 
RecipeList��5O      $��2��6*   $ Title��6�   #��[ 6�   �� 5V      -   5        $ SearchResultsList��5V      $��1��62   $��2��6B   $y�	Collapsed=�6V   $ Title��6m   #��[ 6�   5Y      5Z      5[      