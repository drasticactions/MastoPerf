// See https://aka.ms/new-console-template for more information
using Mastonet.Entities;
using MastoPerf;

Console.WriteLine("Hello, Mastodon!");

var test = new TestViewModel();

test.Statuses.CollectionChanged += (sender, args) =>
{
    if (args.NewItems?.Count > 0)
        Console.WriteLine(((Status)args.NewItems[0]).Content);
};

while(true)
{
}