﻿/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Tizen.Account.FidoClient
{
    /// <summary>
    /// The FIDO message received from the relying party server
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public class UafMessage
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public UafMessage()
        {
        }

        /// <summary>
        /// The FIDO message in JSON format which is received from the relying party server
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string Operation { get; set; }
    }
}