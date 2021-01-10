// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for the loading of images depending on the signatures for the image.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                /// <summary>
                /// Reserved for system use.
                /// </summary>
                [FieldOffset(0)]
                public int Flags;

                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;

                [StructLayout(LayoutKind.Sequential)]
                public struct DUMMYSTRUCT
                {
                    /// <summary>
                    /// MicrosoftSignedOnly : 1
                    /// StoreSignedOnly : 1
                    /// MitigationOptIn : 1
                    /// AuditMicrosoftSignedOnly : 1
                    /// AuditStoreSignedOnly : 1
                    /// ReservedFlags : 27.
                    /// </summary>
                    public uint bitvector;

                    /// <summary>
                    /// Gets or sets a value indicating whether to prevent the process from loading images that are not signed by Microsoft.
                    /// </summary>
                    public bool MicrosoftSignedOnly
                    {
                        get => (this.bitvector & 1u) != 0;

                        set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value indicating whether to prevent the process from loading images that are not signed by the Windows Store.
                    /// </summary>
                    public bool StoreSignedOnly
                    {
                        get => ((this.bitvector & 2u) / 2u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 2u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value indicating whether to prevent the process from loading images that are not signed by Microsoft, the Windows Store and the
                    /// Windows Hardware Quality Labs (WHQL).
                    /// </summary>
                    public bool MitigationOptIn
                    {
                        get => (this.bitvector & 4u) / 4u != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 4u) | this.bitvector;
                    }

                    public bool AuditMicrosoftSignedOnly
                    {
                        get => (this.bitvector & 8u) / 8u != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 8u) | this.bitvector;
                    }

                    public bool AuditStoreSignedOnly
                    {
                        get => (this.bitvector & 16u) / 16u != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 16u) | this.bitvector;
                    }

                    public uint ReservedFlags
                    {
                        get => (this.bitvector & 0xFFFFFFE0u) / 32u;

                        set => this.bitvector = (value * 32u) | this.bitvector;
                    }
                }
            }
        }
    }
}
