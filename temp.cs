private bool GetAvsIcdMessageData(string messageName, ICDData fieldIcdData, string messageTitle, string sn, string bus) 
{
    DataRow[] fieldRows = fieldIcdData.GetDataTable().Select("[식별번호] = '" + messageName + "'", "[Field Number] ASC"); 

    int bytePosition = 0; 

    try 
    {
        foreach(DataRow fieldRow in fieldRows)
        {
            TDAMessageData data = new TDAMessageData(); 

            data.NAME = messageName; 
            data.BYTE_POSITION = bytePosition;
            data.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : stirng.Empty; 
            data.MAPPED_ID = 0; 
            data.SN = fieldRow.IsNull(4) == false ? fieldRow[4].ToString() : string.Empty;
            data.BUS = bus;
            data.FIELD_LABEL = fieldRow.IsNull(4) == false ? fieldRow[4].ToString() : string.Empty;
            data.REMARK = fieldRow.IsNull(11) == false ? fieldRow[11].ToString() : stirng.Empty;

            bytePosition = string.IsNullOrEmpty(fieldRow[6].ToString()) == false ? Convert.ToInt32(fieldRow[6].ToString()) + bytePosition : 0; 

            data.DataKey = new TDAMessageDataKey(data.FIELD_NAME, data.SN, data.BUS); 

            TDAMessageListKey listKey = new TDAMesssageListKey(data.NAME, data.MAPPED_ID, data.BUS); 

            if (this.dicMessageData.ContainsKey(listKey) == false)
            {
                List<TDAMessageData> list = new List<TDAMessageData>(); 
                this.dicMessageData.Add(listKey, list);
            }

            this.dicMessageData[listKey].Add(data);

            ICDData bitIcdData = fieldIcdData.Children[0]; 
            GetAvsIcdMessasgeCode(bitIcdData, fieldRow, data.SN, bus); 
        }
    }

    catch(Exception ex) 
    {
        int a = 0;
    }

    return true;
}

private bool GetAvsIcdMessageCode(ICDData bitIcdData, DataRow fieldRow, string sn, string bus)
{
    DataRow[] bitRows = bitIcdData.GetDataTable().Select("[Field Label] = '" + fieldRow[4].ToString() + "'", "[Bit No] ASC");

    int bitPosition = 0; 
    int bitCount = 1; 
    string compareCode = "@#$#@$%";
    TDAMessageDataKey preDataKey = null; 
    TDAMessageDataKey preDataKeyUDP = null; 

    try 
    {
        TDAMessageCode code = new TDAMessageCode(); 

        code.CODE = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : string.Empty;
        compareCode = code.CODE; 

        code.DESCRIPTION = fieldRow.IsNull(3) == false ? fieldRow[3].ToString() : string.Empty;
        code.ANA_DIS = "ANA";
        code.ANA_DIS_TYPE = fieldRow.IsNull(5) == false ? fieldRow[5].ToString() : string.Empty;
        code.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : string.Empty;

        code.START_BIT = 0; 

        if (string.IsNullOrEmpty(fieldRow[6].ToString()) == true) 
        {
            code.BIT_COUNT = 0; 
        }
        else 
        {
            code.BIT_COUNT = 8 * Convert.ToInt32(fieldRow[6].ToString());
        }

        code.FIELD_BYTE_SIZE = fieldRow.IsNull(6) == false ? Convert.ToInt32(fieldRow[6].ToString()) : -1;
        code.BIT_SWAP = false; 
        code.UNIT = fieldRow.IsNull(9) == false ? fieldRow[9].ToString() : string.Empty;
        code.CALC = fieldRow.IsNull(10) == false ? fieldRow[10].ToString() : string.Empty;

        code.LOWER_RANGE = fieldRow.IsNull(7) == false ? fieldRow[7].ToString() : string.Empty;
        code.UPPER_RANGE = fieldRow.IsNull(8) == false ? fieldRow[8].ToString() : string.Empty;

        code.SN = sn; 
        code.BUS = bus; 

        code.CodeKey = new TDAMessageCodeKey(code.CODE, code.SN, code.BUS, code.FIELD_NAME); 

        TDAMessageDataKey dataKey = new TDAMessageDataKey(code.FIELD_NAME, code.SN, code.BUS);

        if (this.dicMessageCode.ContainsKey(dataKey) == false)
        {
            List<TDAMessageCode> list = new List<TDAMessageCode>(); 
            this.dicMessageCode.Add(dataKey, list);
        }

        this.dicMessageCode[dataKey].Add(code);
        preDataKey = dataKey;

        foreach(DataRow bitRow in bitRows)
        {
            bitCount = 1; 

            code = new TDAMessageCode(); 

            code.CODE = bitRow[2].ToString() + "_" + bitRow[1].ToString(); 
            compareCode = code.CODE;

            code.DESCRIPTION = bitRow.IsNull(2) == false ? bitRow[2].ToString() : string.Empty;
            code.ANA_DIS = "ANA";
            code.ANA_DIS_TYPE = fieldRow.IsNull(5) == false ? fieldRow[5].ToString() : string.Empty;
            code.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : string.Empty;

            code.START_BIT = bitPosition; 
            code.BIT_COUNT = bitCount; 

            bitPosition++;
            bitCount++;

            code.FIELD_BYTE_SIZE = fieldRow.IsNull(6) == false ? Convert.ToInt32(fieldRow[6].ToString()) : -1;
            code.BIT_SWAP = false; 
            code.UNIT = fieldRow.IsNull(9) == false ? fieldRow[9].ToString() : string.Empty;
            code.CALC = fieldRow.IsNull(10) == false ? fieldRow[10].ToString() : string.Empty;

            code.LOWER_RANGE = fieldRow.IsNull(7) == false ? fieldRow[7].ToString() : string.Empty;
            code.UPPER_RANGE = fieldRow.IsNull(8) == false ? fieldRow[8].ToString() : string.Empty;

            code.SN = sn; 
            code.BUS = bus; 

            code.CodeKey = new TDAMessageCodeKey(code.CODE, code.SN, code.BUS, code.FIELD_NAME); 

            TDAMessageDataKey dataKey = new TDAMessageDataKey(code.FIELD_NAME, code.SN, code.BUS);

            if (this.dicMessageCode.ContainsKey(dataKey) == false)
            {
                List<TDAMessageCode> list = new List<TDAMessageCode>(); 
                this.dicMessageCode.Add(dataKey, list);
            }

            this.dicMessageCode[dataKey].Add(code);
            preDataKey = dataKey;

        }
        
    }
    catch (Exception e)
    {
        int a = 0; 
    }

    return true;
}


#region Private Method Get CAN Message 

private bool GetAvsIcdCANMessageData(string messageName, ICDData fieldIcdData, string messageTitle, string sn, string bus) 
{
    DataRow[] fieldRows = fieldIcdData.GetDataTable().Select("[식별번호] = '" + messageName + "'", "[Field Number] ASC");

    int bytePosition = 0; 

    try
    {
        foreach(DataRow fieldRow in fieldRow)
        {
            TDAMessageData data = new TDAMessageData(); 

            data.NAME = messageName; 
            data.BYTE_POSITION = bytePosition;
            data.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : stirng.Empty; 
            data.MAPPED_ID = 0; 
            data.SN = fieldRow.IsNull(4) == false ? fieldRow[4].ToString() : string.Empty;
            data.BUS = bus;
            data.FIELD_LABEL = fieldRow.IsNull(4) == false ? fieldRow[4].ToString() : string.Empty;
            data.REMARK = fieldRow.IsNull(11) == false ? fieldRow[11].ToString() : stirng.Empty;

            bytePosition = string.IsNullOrEmpty(fieldRow[6].ToString()) == false ? Convert.ToInt32(fieldRow[6].ToString()) + bytePosition : 0; 

            data.DataKey = new TDAMessageDataKey(data.FIELD_NAME, data.SN, data.BUS); 

            TDAMessageListKey listKey = new TDAMesssageListKey(data.NAME, data.MAPPED_ID, data.BUS); 

            if (this.dicCANMessageData.ContainsKey(listKey) == false)
            {
                List<TDAMessageData> list = new List<TDAMessageData>(); 
                this.dicCANMessageData.Add(listKey, list);
            }

            this.dicCANMessageData[listKey].Add(data);

            ICDData bitIcdData = fieldIcdData.Children[0]; 
            GetAvsIcdCANMessasgeCode(bitIcdData, fieldRow, data.SN, bus); 
        }
    }
    catch(Exception ex) 
    {
        int a = 0;
    }

    return true;
    
}

private bool GetAvsIcdCANMessageCode(ICDData bitIcdData, DataRow fieldRow, string sn, string bus)
{
    DataRow[] bitRows = bitIcdData.GetDataTable().Select("[Field Label] = '" + fieldRow[4].ToString() + "'", "[Bit No] ASC");

    int bitPosition = 0; 
    int bitCount = 1; 
    string compareCode = "@#$#@$%";
    TDAMessageDataKey preDataKey = null; 
    TDAMessageDataKey preDataKeyUDP = null; 

    try 
    {
        TDAMessageCode code = new TDAMessageCode(); 
        TDAMessageDataKey dataKey = new TDAMessageDataKey();

        if (bitRows.Length > 0)
        {
            foreach(DataRow bitRow in bitRows)
            {
                bitCount = 1; 

                code = new TDAMessageCode(); 

                code.CODE = bitRow[2].ToString() + "_" + bitRow[1].ToString(); 
                compareCode = code.CODE;

                code.DESCRIPTION = bitRow.IsNull(2) == false ? bitRow[2].ToString() : string.Empty;
                code.ANA_DIS = "ANA";
                code.ANA_DIS_TYPE = fieldRow.IsNull(5) == false ? fieldRow[5].ToString() : string.Empty;
                code.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : string.Empty;

                code.START_BIT = bitPosition; 
                code.BIT_COUNT = bitCount; 

                bitPosition++;
                bitCount++;

                code.FIELD_BYTE_SIZE = fieldRow.IsNull(6) == false ? Convert.ToInt32(fieldRow[6].ToString()) : -1;
                code.BIT_SWAP = false; 
                code.UNIT = fieldRow.IsNull(9) == false ? fieldRow[9].ToString() : string.Empty;
                code.CALC = fieldRow.IsNull(10) == false ? fieldRow[10].ToString() : string.Empty;

                code.LOWER_RANGE = fieldRow.IsNull(7) == false ? fieldRow[7].ToString() : string.Empty;
                code.UPPER_RANGE = fieldRow.IsNull(8) == false ? fieldRow[8].ToString() : string.Empty;

                code.SN = sn; 
                code.BUS = bus; 

                code.CodeKey = new TDAMessageCodeKey(code.CODE, code.SN, code.BUS, code.FIELD_NAME); 

                TDAMessageDataKey dataKey = new TDAMessageDataKey(code.FIELD_NAME, code.SN, code.BUS);

                if (this.dicCANMessageCode.ContainsKey(dataKey) == false)
                {
                    List<TDAMessageCode> list = new List<TDAMessageCode>(); 
                    this.dicCANMessageCode.Add(dataKey, list);
                }

                this.dicCANMessageCode[dataKey].Add(code);
                preDataKey = dataKey;

            }
        }
        else 
        {
            code.CODE = fieldRow[2].ToString() + "_" + bitRow[1].ToString();
            compareCode = code.CODE; 

            code.DESCRIPTION = bitRow.IsNull(2) == false ? bitRow[2].ToString() : string.Empty;
            code.ANA_DIS = "ANA";
            code.ANA_DIS_TYPE = fieldRow.IsNull(5) == false ? fieldRow[5].ToString() : string.Empty;
            code.FIELD_NAME = fieldRow.IsNull(2) == false ? fieldRow[2].ToString() : string.Empty;

            code.START_BIT = 0; 

            if (string.IsNullOrEmpty(fieldRow[6].ToString()) == true) 
            {
                code.BIT_COUNT = 0; 
            }
            else 
            {
                code.BIT_COUNT = 8 * Convert.ToInt32(fieldRow[6].ToString());
            }

            code.FIELD_BYTE_SIZE = fieldRow.IsNull(6) == false ? Convert.ToInt32(fieldRow[6].ToString()) : -1;
            code.BIT_SWAP = false; 
            code.UNIT = fieldRow.IsNull(9) == false ? fieldRow[9].ToString() : string.Empty;
            code.CALC = fieldRow.IsNull(10) == false ? fieldRow[10].ToString() : string.Empty;

            code.LOWER_RANGE = fieldRow.IsNull(7) == false ? fieldRow[7].ToString() : string.Empty;
            code.UPPER_RANGE = fieldRow.IsNull(8) == false ? fieldRow[8].ToString() : string.Empty;

            code.SN = sn; 
            code.BUS = bus; 

            code.CodeKey = new TDAMessageCodeKey(code.CODE, code.SN, code.BUS, code.FIELD_NAME); 

            TDAMessageDataKey dataKey = new TDAMessageDataKey(code.FIELD_NAME, code.SN, code.BUS);

            if (this.dicCANMessageCode.ContainsKey(dataKey) == false)
            {
                List<TDAMessageCode> list = new List<TDAMessageCode>(); 
                this.dicCANMessageCode.Add(dataKey, list);
            }

            this.dicCANMessageCode[dataKey].Add(code);
            preDataKey = dataKey;

        }
    }
    catch (Exception e)
    {
        int a = 0; 
    }

    return true;
}

#endregion

