using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imohsenb.iso8583.builders;

namespace imohsenb.iso8583.interfaces
{
    public interface MessageClass
    {
        /**
         * Determine if funds are available, get an approval but do not post to account for reconciliation.
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> Authorization();

        /**
         * Determine if funds are available, get an approval and post directly to the account.
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> Financial();

        /**
         * Used for hot-card, TMS and other exchanges
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> FileAction();

        /**
         * Reverses the action of a previous authorization.
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> Reversal();

        /**
         * Transmits settlement information label.
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> Reconciliation();

        /**
         * Transmits administrative advice.
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> Administrative();
        MessagePacker<GeneralMessageClassBuilder> FeeCollection();

        /**
         * Used for secure key exchange, logon, echo test and other network functions
         * @return
         */
        MessagePacker<GeneralMessageClassBuilder> NetworkManagement();
    }

}
