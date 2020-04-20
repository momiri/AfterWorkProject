using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AfterWorkItLink
{
    public class ListeLinkersInvited : INotifyPropertyChanged
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Calendar API Quickstart";

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string str="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        private ObservableCollection<Linker> myListLinkers;
        public ObservableCollection<Linker> MyListLinkers
        {
            get
            {
                return myListLinkers;
            }
            set
            {
                if(value != myListLinkers)
                {
                    myListLinkers = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Linker linkerSelectionnee;
        public Linker LinkerSelectionnee
        {
            get
            {
                return linkerSelectionnee;
            }
            set
            {
                if(value != linkerSelectionnee)
                {
                    linkerSelectionnee = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ListeLinkersInvited() //test commit
        {
            MyListLinkers = new ObservableCollection<Linker>();
            //LinkerSelectionnee = new Linker() { Email = "momiei", Status = "yes" };

            //MyListLinkers.Add(linkerSelectionnee);
            Dictionary<string, string> myList = GetListGuestsLinker();
            foreach (KeyValuePair<string, string> item in myList) 
            {
                string colorStatus = "";
                if (item.Value == "accepted")
                {
                    colorStatus = "Green";
                }else if(item.Value == "declined")
                {
                    colorStatus = "Red";
                }
                else
                {
                    colorStatus = "Blue";
                }
                LinkerSelectionnee = new Linker() { Email = item.Key, Status = item.Value, ColorStatus = colorStatus };
                MyListLinkers.Add(LinkerSelectionnee);
            }
        }


        private Dictionary<string, string> GetListGuestsLinker()
        {
            Dictionary<string, string> myList = new Dictionary<string, string>();
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open,
                FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment
                  .SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                   GoogleClientSecrets.Load(stream).Secrets,
                  Scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Calendar Service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Console.WriteLine("Upcoming events:");
            Events events = request.Execute();
            if (events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    string titleOfEvent = (eventItem.Summary).ToUpper();
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    Console.WriteLine("*******{0}", eventItem.Created.Value.Date);
                    Console.WriteLine("{0} ({1})", eventItem.Creator.Email, when);
                    Console.WriteLine("{0} ({1})", eventItem.Attendees.Count, when);
                  //  if (titleOfEvent.Contains("AFTER"))
                  //  {
                        foreach (var attende in eventItem.Attendees)
                        {
                            Console.WriteLine("--------- {0} --- {1} --- {2}", attende.Email, attende.ResponseStatus, attende.Comment);

                            myList.Add(attende.Email, attende.ResponseStatus);
                        }
                   // }
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
            Console.Read();

            return myList;
        }
    }

    public class Linker : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string email;
        private string status;
        private string colorStatus;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                {
                    email = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("email"));
                    }
                }
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if(value != status)
                {
                    status = value;
                    if(PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("status"));
                    }
                }
            }
        }

        public string ColorStatus 
        {
            get
            {
                return colorStatus;
            }
            set
            {
                if(value != colorStatus)
                {
                    colorStatus = value;
                    if(PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("colorStatus"));
                    }
                }
            }
        }
    }


}
