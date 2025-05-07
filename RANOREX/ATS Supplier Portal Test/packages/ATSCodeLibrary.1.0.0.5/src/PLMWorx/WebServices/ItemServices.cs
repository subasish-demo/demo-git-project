using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.PLMWorx.WebServices
{
    public class ItemServices
    {
        private Item item;

        /// <summary>
        /// Provides methods for getting and modifying item version records in PLMWorx
        /// </summary>
        /// <param name="itemNumber">Item number</param>
        /// <param name="itemMajorRevision">Item Major Revision</param>
        /// <param name="itemMinorRevision">Item Minor Revision</param>
        /// <param name="itemVersion">Item Version</param>
        public ItemServices(string itemNumber, string itemMajorRevision, string itemMinorRevision, string itemVersion)
        {
            item = new Item()
            {
                itemNumber = itemNumber,
                itemMajorRevision = itemMajorRevision,
                itemMinorRevision = itemMinorRevision,
                itemVersion = itemVersion
            };
        }

        private class Item
        {
            internal string itemNumber;
            internal string itemMajorRevision;
            internal string itemMinorRevision;
            internal string itemVersion;
        }

        /// <summary>
        /// Add a webaddress link to an item's supporting documents
        /// </summary>
        /// <param name="URL">Address to add as link</param>
        /// <returns>True if succussful</returns>
        public bool AddUriToItem(string URL)
        {
            string val = ServerInfo.SubmitRequest(HttpMethod.Post, $"/atsplmworxapi/add_url_to_item?item_number={item.itemNumber}&item_major_revision={item.itemMajorRevision}&item_minor_revision={item.itemMinorRevision}&item_version={item.itemVersion}&file_path={URL}");
            if (val.Contains("document_version_id"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add a comment to an item's history
        /// </summary>
        /// <param name="comment">Comment to show on history tab of item</param>
        /// <returns>True if successful</returns>
        public bool UpdateItemHistory(string comment)
        {
            string val = ServerInfo.SubmitRequest(HttpMethod.Post, $"/atsplmworxapi/update_item_history?item_number={item.itemNumber}&item_major_revision={item.itemMajorRevision}&item_minor_revision={item.itemMinorRevision}&item_version={item.itemVersion}&comment={comment}");
            return val == "1";
        }
    }
}
