Public Module Query

    Public Function selectCheckUpdateFlg(playerNo As Integer) As String
        Return "SELECT update_flg FROM update_check WHERE id =" & playerNo & ";"
    End Function

    Public Function updCheckUpdateFlg(playerNo As Integer, flg As Integer) As String
        Return "UPDATE update_check SET update_flg = " & flg & " WHERE id = " & playerNo & ";"
    End Function

End Module
