using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Drastic.FDTemplateLayoutCell;
using Drastic.PureLayout;
using Masonry;
using Mastonet.Entities;
using ObjCRuntime;
using UIKit;

namespace MastoPerf.Apple
{
	public class TimelineViewController : UIViewController
	{
        private TimelineTableView tableView;
        private TestViewModel viewModel;
        private TableSource source;
        public TimelineViewController()
		{
            this.viewModel = new TestViewModel();
            this.tableView = new TimelineTableView(this.View!.Frame, UITableViewStyle.Plain);
            this.source = new TableSource(this.tableView, this.viewModel.Statuses);
            this.tableView.Source = this.source;

            this.View!.AddSubview(this.tableView);
            this.tableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.tableView.AutoPinEdgesToSuperviewEdges();

            this.viewModel.StartStreaming();
        }

		public class TimelineTableView : UITableView
		{
            public TimelineTableView(CGRect rect, UITableViewStyle style)
                : base(rect, style)
            {
                this.SetFd_debugLogEnabled(true);
                this.RegisterClassForCellReuse(typeof(ItemViewCell), ItemViewCell.PublicReuseIdentifier);
            }
        }

        private class TableSource : UITableViewSource
        {
            private ObservableCollection<Status> Status { get; }
            public TableSource(UITableView tableView, ObservableCollection<Status> statuses)
            {
                this.TableView = tableView;
                this.Status = statuses;
                this.Status.CollectionChanged += Status_CollectionChanged;
            }

            public nint Count => this.Status.Count;

            public UITableView TableView { get; }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return this.Status.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                ItemViewCell? cell = tableView.DequeueReusableCell(ItemViewCell.PublicReuseIdentifier) as ItemViewCell;
                Status item = this.Status[indexPath.Row];
                if (cell == null)
                {
                    cell = new ItemViewCell(ItemViewCell.PublicReuseIdentifier);
                }

                cell.SetupCell(item);

                return cell;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return this.TableView.Fd_heightForCellWithIdentifier(ItemViewCell.PublicReuseIdentifier, indexPath, (cell) => {
                    if (cell is ItemViewCell feedCell)
                    {
                        feedCell.SetupCell(this.Status[indexPath.Row]);
                    }
                });
            }

            private void Status_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var newIndexPaths = CreateNSIndexPathArray(args.NewStartingIndex, args.NewItems!.Count);
                        TableView.InsertRows(newIndexPaths, UITableViewRowAnimation.Automatic);
                       // TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        var oldIndexPaths = CreateNSIndexPathArray(args.OldStartingIndex, args.OldItems!.Count);
                        TableView.DeleteRows(oldIndexPaths, UITableViewRowAnimation.Automatic);
                        break;
                }
            }

            private NSIndexPath[] CreateNSIndexPathArray(int startingPosition, int count)
            {
                var newIndexPaths = new NSIndexPath[count];
                for (var i = 0; i < count; i++)
                {
                    newIndexPaths[i] = NSIndexPath.FromRowSection(i + startingPosition, 0);
                }

                return newIndexPaths;
            }
        }

        private class ItemViewCell : UITableViewCell
        {
            private UIImageView icon = new UIImageView();

            private Status? item;

            private UILabel TitleLabel { get; set; }
            private UILabel ContentLabel { get; set; }
            private UIImageView ContentImageView { get; set; }
            private UILabel UserLabel { get; set; }
            private UILabel TimeLabel { get; set; }

            /// <summary>
            /// Gets the Reuse Identifier.
            /// </summary>
            public static NSString PublicReuseIdentifier => new NSString("ItemViewCell");

            public ItemViewCell(NSString reuseIdentifier) : base(UITableViewCellStyle.Default, reuseIdentifier)
            {
                this.icon.Layer.CornerRadius = 5;
                this.icon.Layer.MasksToBounds = true;
                this.SetupUI();
                this.SetupLayout();
            }

            protected internal ItemViewCell(NativeHandle handle) : base(handle)
            {
                this.icon.Layer.CornerRadius = 5;
                this.icon.Layer.MasksToBounds = true;
                this.SetupUI();
                this.SetupLayout();
            }


            public void SetupUI()
            {
                UILabel titleLabel = new UILabel();
                this.ContentView.AddSubview(titleLabel);
                this.TitleLabel = titleLabel;

                UILabel contentlabel = new UILabel()
                {
                    Lines = 0
                };
                this.ContentView.AddSubview(contentlabel);
                this.ContentLabel = contentlabel;

                UIImageView contentImageView = new UIImageView()
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit
                };
                this.ContentView.AddSubview(contentImageView);
                this.ContentImageView = contentImageView;

                UILabel userLabel = new UILabel();
                this.ContentView.AddSubview(userLabel);
                this.UserLabel = userLabel;

                UILabel timeLabel = new UILabel();
                this.ContentView.AddSubview(timeLabel);
                this.TimeLabel = timeLabel;
            }

            public void SetupLayout()
            {
                int margin = 4;
                int padding = 10;

                this.TitleLabel.MakeConstraints(make => {
                    make.Top.And.Left.EqualTo(this.ContentView).With.Offset(padding);
                    make.Right.EqualTo(this.ContentView.Right()).With.Offset(-padding);
                });

                this.ContentLabel.MakeConstraints(make => {
                    make.Left.And.Right.EqualTo(this.TitleLabel);
                    make.Top.EqualTo(this.TitleLabel.Bottom()).With.Offset(margin);
                });

                this.ContentImageView.MakeConstraints(make => {
                    make.Left.EqualTo(this.TitleLabel.Left());
                    make.Top.EqualTo(this.ContentLabel.Bottom()).With.Offset(margin);
                });

                this.UserLabel.MakeConstraints(make => {
                    make.Left.EqualTo(this.TitleLabel.Left());
                    make.Top.EqualTo(this.ContentImageView.Bottom()).With.Offset(margin);
                    make.Bottom.EqualTo(this.ContentView.Bottom()).With.Offset(-margin);
                });

                this.TimeLabel.MakeConstraints(make => {
                    make.Bottom.And.Top.EqualTo(this.UserLabel);
                    make.Right.EqualTo(this.TitleLabel.Right());
                });
            }

            public void SetupCell(Status item)
            {
                this.item = item;

                this.UserLabel.Text = item.Account.UserName;
                this.TitleLabel.Text = item.Account.AccountName;
                this.TimeLabel.Text = item.CreatedAt.ToShortDateString();

                this.ContentLabel.Text = item.Content;
            }
        }
    }
}

