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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OcppTestTool.UserControls
{
    /// <summary>
    /// LoadingOverlay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingOverlay : UserControl
    {
        public LoadingOverlay()
        {
            InitializeComponent();
        }

        // 열림/닫힘
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(LoadingOverlay),
                new PropertyMetadata(false));

        // 메시지
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(LoadingOverlay),
                new PropertyMetadata("처리 중입니다..."));

        // 배경(오버레이) 브러시 & 불투명도
        public Brush OverlayBrush
        {
            get => (Brush)GetValue(OverlayBrushProperty);
            set => SetValue(OverlayBrushProperty, value);
        }
        public static readonly DependencyProperty OverlayBrushProperty =
            DependencyProperty.Register(nameof(OverlayBrush), typeof(Brush), typeof(LoadingOverlay),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)))); // 기본: Black

        public double OverlayOpacity
        {
            get => (double)GetValue(OverlayOpacityProperty);
            set => SetValue(OverlayOpacityProperty, value);
        }
        public static readonly DependencyProperty OverlayOpacityProperty =
            DependencyProperty.Register(nameof(OverlayOpacity), typeof(double), typeof(LoadingOverlay),
                new PropertyMetadata(0.25d));

        // 가운데 카드 배경
        public Brush CardBrush
        {
            get => (Brush)GetValue(CardBrushProperty);
            set => SetValue(CardBrushProperty, value);
        }
        public static readonly DependencyProperty CardBrushProperty =
            DependencyProperty.Register(nameof(CardBrush), typeof(Brush), typeof(LoadingOverlay),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));
    }
}
