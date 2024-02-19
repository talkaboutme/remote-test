Private Sub btn_exportExcel_Click()


' 적용하고자 하는 엑셀파일을 선택할 수 있는 대화상자를 연 뒤 엑셀파일을 불러온다. 
Dim fd As Office.FileDialog
Set fd = Application.FileDialog(msoFileDialogFilePicker)

Dim MyXL As Excel.Application
Dim exportSheet As Object

'progress bar 정의 
Dim cProg_export As New clsLblProg
Dim cProg_header As New clsLblProg
Dim cProg_field As New clsLblProg

Dim ICDList() As String 
Dim listSize As Integer 
listSize = 0 

'전체추가 체크박스가 체크되어 있는지 확인한다. 
If Me.chk_exportAll.Value = 0 Then 
    '리스트 박스 내 목록이 있는지 확인
    If Me.chk_exportICDList.ListCount = 0 then 
        Msgbox "추출할 ICD 목록을 선택한 뒤 진행하세요. " 
        Exit Sub 
    Else
        For listSize = 0 To Me.listbox_exportICDList.ListCount - 1

            ReDim Preserve ICDList(listSize)
            ICDName = Me.listbox_exportICDList.ItemData(i)
            ICDName = Left(ICDName, Len(ICDName) - 7)
            ICDList(listSize) = ICDName
        Next listSize

    End If
Else 


End If




