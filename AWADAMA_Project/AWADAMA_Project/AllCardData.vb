
Public Class AllCardData
    Public Shared Property Data As List(Of CardDataDto) = New List(Of CardDataDto)

    Public Shared Sub GetData()
        SetData(Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_DECK, "A2:O" & Constant.MAX_CARD_CELL))
    End Sub

    Public Shared Sub SetData(all_card_data As IList(Of IList(Of Object)))
        For Each row As IList(Of Object) In all_card_data
            Dim card_dto As New CardDataDto(
                no:=If(row.Count > 0, row(0).ToString(), ""),
                name:=If(row.Count > 1, row(1).ToString(), ""),
                cardname:=If(row.Count > 2, row(2).ToString(), ""),
                cost:=If(row.Count > 3, row(3).ToString(), ""),
                type:=If(row.Count > 4, row(4).ToString(), ""),
                line:=If(row.Count > 5, row(5).ToString(), ""),
                atack:=If(row.Count > 6, row(6).ToString(), ""),
                defense:=If(row.Count > 7, row(7).ToString(), ""),
                intellect:=If(row.Count > 8, row(8).ToString(), ""),
                magic:=If(row.Count > 9, row(9).ToString(), ""),
                effect:=If(row.Count > 10, row(10).ToString(), ""),
                flavor:=If(row.Count > 11, row(11).ToString(), ""),
                skill1:=If(row.Count > 12, row(12).ToString(), ""),
                skill2:=If(row.Count > 13, row(13).ToString(), ""),
                skill3:=If(row.Count > 14, row(14).ToString(), "")
        )
            Data.Add(card_dto)
        Next
    End Sub

End Class
