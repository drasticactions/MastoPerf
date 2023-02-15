using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MastoPerf
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private int _count = 0;
        private TimelineStreaming streaming;
        private HttpClient client;
        public TestViewModel()
        {
            this.client = new HttpClient();
            this.Statuses = new ObservableCollection<Status>();
            var client = new MastodonClient("mastodon.social", string.Empty, this.client);
            this.streaming = client.GetPublicStreaming();
            this.streaming.OnUpdate += this.StreamingOnUpdate;
            this.streaming.OnDelete += this.StreamingOnDelete;
            this.streaming.OnConversation += this.StreamingOnConversation;
            this.streaming.OnNotification += this.StreamingOnNotification;
            this.streaming.OnFiltersChanged += this.StreamingOnFiltersChanged;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Count
        {
            get { return this._count; }
            set { this.SetProperty(ref this._count, value); }
        }

        public int StatusCollectionCount => this.Statuses.Count;

        public void StartStreaming()
            => this.streaming.Start();

        public void EndStreaming()
           => this.streaming.Stop();

        public ObservableCollection<Status> Statuses { get; }

        private void StreamingOnFiltersChanged(object? sender, StreamFiltersChangedEventArgs e)
        {
        }

        private void StreamingOnNotification(object? sender, StreamNotificationEventArgs e)
        {
        }

        private void StreamingOnConversation(object? sender, StreamConversationEvenTargs e)
        {
        }

        private void StreamingOnDelete(object? sender, StreamDeleteEventArgs e)
        {
            if (this.Statuses.FirstOrDefault(n => n.Id == e.StatusId.ToString()) is Status status)
            {
                this.Statuses.Remove(status);
            }
        }

        private void StreamingOnUpdate(object? sender, StreamUpdateEventArgs e)
        {
            this.Statuses.Insert(0, e.Status);

            if (this.Statuses.Count > 250)
                this.Statuses.RemoveAt(this.Statuses.Count - 1);

            this.Count = this.Count + 1;
            this.OnPropertyChanged(nameof(StatusCollectionCount));
        }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
