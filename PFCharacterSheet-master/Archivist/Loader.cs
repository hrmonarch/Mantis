﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using CharacterDataObjects;
using CharacterDataObjects.CharacterDataElements;
using log4net;
using System.Collections.Generic;

namespace Archivist
{
    public static class Loader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Loader));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void GetRemoteUserAccountsFromFile(string value)
        {
            // Try to get the file - It may be locked (in use)
            try
            {
                using (StreamReader sr = File.OpenText(value))
                {
                    while (!sr.EndOfStream)
                    {
                        // First line expected to be Skills
                        var line = sr.ReadLine();

                        try
                        {
                            // Out with the old...
                            MyConnections.AllRemoteUserAccounts.Clear();

                            // create a temp container
                            var x = new ObservableCollection<RemoteUserAccount>();

                            // fill our temp container
                            x = JsonConvert.DeserializeObject<ObservableCollection<RemoteUserAccount>>(line);

                            // worth noting that ObservableCollections will 'auto update' this way and not if you 
                            // directly assign to it; i.e.  Character.Skills = JsonConvert.DeserializeObject<ObservableCollection<Skill>>(line);
                            // we want to change our collection container, not replace it.
                            foreach(var elem in x){
                                MyConnections.AllRemoteUserAccounts.Add(elem);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Error when parsing line for AllRemoteUserAccounts. \n{0}\n{1}", line, ex);
                            // TODO -- consider -- : Notify user that the character data may be incomplete.
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failure to read from selected character file.\n{0}", ex);
                // log an error has occured in loading of charcter file
                // notify user that the file may be locked and to close any currently open instance of the file
            }
        }
    }
}
