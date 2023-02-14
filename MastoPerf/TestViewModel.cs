using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoPerf
{
    public class TestViewModel
    {
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
            this.streaming.Start();
        }

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
        }
    }
}
