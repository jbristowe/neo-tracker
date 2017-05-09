using NEOTracker.Data;
using System;
using Telerik.UI.Xaml.Controls.Chart;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NEOTracker.Views
{
    public sealed partial class TorinoImpactHazardScaleViewPage : Page
    {
        public TorinoImpactHazardScaleViewPage()
        {
            InitializeComponent();
            chart.Loaded += ChartLoaded;
            chart.SizeChanged += ChartSizeChanged;
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            DrawAnnotations();
        }

        private void ChartSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAnnotations();
        }

        private void DrawAnnotations()
        {
            chart.Annotations.Clear();

            chart.Annotations.Add(new CartesianMarkedZoneAnnotation() { HorizontalFrom = 1, HorizontalTo = 100, VerticalFrom = 1000, VerticalTo = 10000, Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)) });
            chart.Annotations.Add(new CartesianMarkedZoneAnnotation() { HorizontalFrom = 1, HorizontalTo = 100, VerticalFrom = 100, VerticalTo = 1000, Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)) });
            chart.Annotations.Add(new CartesianMarkedZoneAnnotation() { HorizontalFrom = 1, HorizontalTo = 100, VerticalFrom = 20, VerticalTo = 100, Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)) });
            chart.Annotations.Add(new CartesianMarkedZoneAnnotation() { HorizontalFrom = 0.01, HorizontalTo = 1, VerticalFrom = 1000, VerticalTo = 10000, Fill = new SolidColorBrush(Color.FromArgb(100, 255, 165, 0)) });
            chart.Annotations.Add(new CartesianMarkedZoneAnnotation() { HorizontalFrom = 0.01, HorizontalTo = 1, VerticalFrom = 20, VerticalTo = 100, Fill = new SolidColorBrush(Color.FromArgb(100, 255, 255, 0)) });

            DrawTorinoImpactLevelOne();
            DrawTorinoImpactLevelTwo();
            DrawTorinoImpactLevelFour();
            DrawTorinoImpactLevelFive();
            DrawTorinoImpactLevelSix();
        }

        private void DrawTorinoImpactLevelOne()
        {
            var origin = chart.ConvertDataToPoint(Tuple.Create<object, object>(1E-08, 10000));

            var vertexA = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.002, 20));
            vertexA.X -= origin.X;
            vertexA.Y -= origin.Y;

            var vertexB = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 20));
            vertexB.X -= origin.X;
            vertexB.Y -= origin.Y;

            var vertexC = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 100));
            vertexC.X -= origin.X;
            vertexC.Y -= origin.Y;

            var vertexD = chart.ConvertDataToPoint(Tuple.Create<object, object>(1E-06, 10000));
            vertexD.X -= origin.X;
            vertexD.Y -= origin.Y;

            var xamlString = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Viewbox Stretch=""Fill"">
                        <TextBlock Text=""This is custom annotation with ContentTemplate"" TextWrapping=""Wrap"" TextAlignment=""Center"" VerticalAlignment=""Center"" HorizontalAlignment=""Center"" />
                        <Path Fill=""#6400FF00"">
                            <Path.Data>
                                <PathGeometry>
                                    <PathFigure StartPoint=""0,0"">"
                                        + @"<LineSegment Point=""" + vertexA + @""" />"
                                        + @"<LineSegment Point=""" + vertexB + @""" />"
                                        + @"<LineSegment Point=""" + vertexC + @""" />"
                                        + @"<LineSegment Point=""" + vertexD + @""" />" +
                                    @"</PathFigure>
                                </PathGeometry>
                            </Path.Data>
                        </Path>
                    </Viewbox>
                </DataTemplate>";
            var dataTemplate = XamlReader.Load(xamlString) as DataTemplate;

            var annotation = new CartesianCustomAnnotation();
            annotation.HorizontalValue = 1E-08;
            annotation.VerticalValue = 10000;
            annotation.ContentTemplate = dataTemplate;
            chart.Annotations.Add(annotation);
        }

        private void DrawTorinoImpactLevelTwo()
        {
            var origin = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.000001, 10000));

            var vertexA = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 100));
            vertexA.X -= origin.X;
            vertexA.Y -= origin.Y;

            var vertexB = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 1000));
            vertexB.X -= origin.X;
            vertexB.Y -= origin.Y;

            var vertexC = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.0001, 10000));
            vertexC.X -= origin.X;
            vertexC.Y -= origin.Y;

            var xamlString = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Viewbox Stretch=""Fill"">
                        <Path Fill=""#64FFFF00"">
                            <Path.Data>
                                <PathGeometry>
                                    <PathFigure StartPoint=""0,0"">"
                                        + @"<LineSegment Point=""" + vertexA + @""" />"
                                        + @"<LineSegment Point=""" + vertexB + @""" />"
                                        + @"<LineSegment Point=""" + vertexC + @""" />" +
                                    @"</PathFigure>
                                </PathGeometry>
                            </Path.Data>
                        </Path>
                    </Viewbox>
                </DataTemplate>";
            var dataTemplate = XamlReader.Load(xamlString) as DataTemplate;

            var annotation = new CartesianCustomAnnotation();
            annotation.HorizontalValue = 0.000001;
            annotation.VerticalValue = 10000;
            annotation.ContentTemplate = dataTemplate;
            chart.Annotations.Add(annotation);
        }

        private void DrawTorinoImpactLevelFour()
        {
            var origin = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 1000));

            var vertexA = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 100));
            vertexA.X -= origin.X;
            vertexA.Y -= origin.Y;

            var vertexB = chart.ConvertDataToPoint(Tuple.Create<object, object>(1, 100));
            vertexB.X -= origin.X;
            vertexB.Y -= origin.Y;

            var xamlString = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Viewbox Stretch=""Fill"">
                        <Path Fill=""#64FFFF00"">
                            <Path.Data>
                                <PathGeometry>
                                    <PathFigure StartPoint=""0,0"">"
                                        + @"<LineSegment Point=""" + vertexA + @""" />"
                                        + @"<LineSegment Point=""" + vertexB + @""" />" +
                                    @"</PathFigure>
                                </PathGeometry>
                            </Path.Data>
                        </Path>
                    </Viewbox>
                </DataTemplate>";
            var dataTemplate = XamlReader.Load(xamlString) as DataTemplate;

            var annotation = new CartesianCustomAnnotation();
            annotation.HorizontalValue = 0.01;
            annotation.VerticalValue = 1000;
            annotation.ContentTemplate = dataTemplate;
            chart.Annotations.Add(annotation);
        }

        private void DrawTorinoImpactLevelFive()
        {
            var origin = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 1000));

            var vertexA = chart.ConvertDataToPoint(Tuple.Create<object, object>(1, 100));
            vertexA.X -= origin.X;
            vertexA.Y -= origin.Y;

            var vertexB = chart.ConvertDataToPoint(Tuple.Create<object, object>(1, 1000));
            vertexB.X -= origin.X;
            vertexB.Y -= origin.Y;

            var xamlString = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Viewbox Stretch=""Fill"">
                        <Path Fill=""#64FFA500"">
                            <Path.Data>
                                <PathGeometry>
                                    <PathFigure StartPoint=""0,0"">"
                                        + @"<LineSegment Point=""" + vertexA + @""" />"
                                        + @"<LineSegment Point=""" + vertexB + @""" />" +
                                    @"</PathFigure>
                                </PathGeometry>
                            </Path.Data>
                        </Path>
                    </Viewbox>
                </DataTemplate>";
            var dataTemplate = XamlReader.Load(xamlString) as DataTemplate;

            var annotation = new CartesianCustomAnnotation();
            annotation.HorizontalValue = 0.01;
            annotation.VerticalValue = 1000;
            annotation.ContentTemplate = dataTemplate;
            chart.Annotations.Add(annotation);
        }

        private void DrawTorinoImpactLevelSix()
        {
            var origin = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.0001, 10000));

            var vertexA = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 1000));
            vertexA.X -= origin.X;
            vertexA.Y -= origin.Y;

            var vertexB = chart.ConvertDataToPoint(Tuple.Create<object, object>(0.01, 10000));
            vertexB.X -= origin.X;
            vertexB.Y -= origin.Y;

            var xamlString = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Viewbox Stretch=""Fill"">
                        <Path Fill=""#64FFA500"">
                            <Path.Data>
                                <PathGeometry>
                                    <PathFigure StartPoint=""0,0"">"
                                        + @"<LineSegment Point=""" + vertexA + @""" />"
                                        + @"<LineSegment Point=""" + vertexB + @""" />" +
                                    @"</PathFigure>
                                </PathGeometry>
                            </Path.Data>
                        </Path>
                    </Viewbox>
                </DataTemplate>";
            var dataTemplate = XamlReader.Load(xamlString) as DataTemplate;

            var annotation = new CartesianCustomAnnotation();
            annotation.HorizontalValue = 0.0001;
            annotation.VerticalValue = 10000;
            annotation.ContentTemplate = dataTemplate;
            chart.Annotations.Add(annotation);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var esaData = await EsaDataManager.GetEsaDataAsync();
            DataContext = esaData;
        }
    }
}
