namespace CrestronInfluxDB;
        // class declarations
         class HttpAPI;
     class HttpAPI 
    {
        // class delegates
        delegate FUNCTION DelegateString ( SIMPLSHARPSTRING str );

        // class events

        // class functions
        static FUNCTION TCPClientSettings ( STRING _ip , INTEGER _prt , STRING _dbname );
        static FUNCTION InfluxDBPost ( STRING Content );
        STRING_FUNCTION ToString ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty DelegateString CommandStatus;
    };

