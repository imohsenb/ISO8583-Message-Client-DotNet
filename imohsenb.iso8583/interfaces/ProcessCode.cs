using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imohsenb.iso8583.enums;

namespace imohsenb.iso8583.interfaces
{
    public interface ProcessCode<T>
    {
        DataElement<T> ProcessCode(string code) ;
        DataElement<T> ProcessCode(PC_TTC_100 ttc) ;
        DataElement<T> ProcessCode(PC_TTC_100 ttc, PC_ATC atcFrom, PC_ATC atcTo) ;
        DataElement<T> ProcessCode(PC_TTC_200 ttc) ;
        DataElement<T> ProcessCode(PC_TTC_200 ttc, PC_ATC atcFrom, PC_ATC atcTo) ;
    }

}
