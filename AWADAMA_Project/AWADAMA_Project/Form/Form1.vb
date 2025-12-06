Public Class Form1
    Dim handDataNameList As New ArrayList(Constant.HandCardNum)
    Dim HandData As List(Of CardDataDto) = New List(Of CardDataDto)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AllCardData.GetData()

        handDataNameList.AddRange(New String(Constant.HandCardNum - 1) {})
    End Sub

    Private Sub DeckShuffle_Button_Click(sender As Object, e As EventArgs) Handles btShuffleButton.Click

        Dim result As MsgBoxResult = MsgBox("デッキをシャッフルしますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")

        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim deck As IList(Of IList(Of Object))
        Dim archive As IList(Of IList(Of Object))

        deck = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_DECK, "A2:C" & Constant.MAX_CARD_CELL)

        archive = Common.ListShuffle(Of IList(Of Object))(deck)

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "A2:C" & Constant.MAX_CARD_CELL, archive)

    End Sub

    Private Sub Deck_Click(sender As Object, e As EventArgs) Handles btDeck.Click

        If (HandData.Count >= Constant.HandCardNum) Then
            Exit Sub
        End If

        Dim archive As IList(Of IList(Of Object))

        archive = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "A2:C" & Constant.MAX_CARD_CELL)

        Dim index As Integer = 1
        Dim drawIndex As Integer = 1
        Dim disCartCount As Integer = 1
        Dim isGet As Boolean = False
        For Each card As IList(Of Object) In archive
            If (card.Count = 2) Then
                If (Not isGet) Then
                    handDataNameList.SetRange(index - 1, {card(1).ToString})
                    isGet = True
                    drawIndex = index
                End If
            Else
                If (card(2).ToString = "1") Then
                    disCartCount += 1
                End If
            End If
            index += 1
        Next

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "C" & (drawIndex + 1), "1")

        btDeck.Text = (Constant.MAX_DECK_COUNT - disCartCount).ToString()

        HandData.Clear()
        For Each handName As String In handDataNameList
            If (handName <> Nothing) Then
                HandData.Add(GetCardDataByName(handName))
            End If
        Next

        HandCardUpdate()

        If (HandData.Count >= Constant.HandCardNum) Then
            lbHandMaxSign.Visible = True
        End If
    End Sub

    Private Sub pnHand_Click(sender As Object, e As EventArgs) Handles pnHand1.Click, pnHand2.Click, pnHand3.Click, pnHand4.Click, pnHand5.Click, pnHand6.Click
        Dim clickedPanel = TryCast(sender, Panel)

        pnSelectPanelColor.Left = clickedPanel.Location.X - 5
        pnSelectPanelColor.Top = clickedPanel.Location.Y - 5
        pnSelectPanelColor.Visible = True

        Dim clickCardName = ""
        If clickedPanel.Name.Substring(0, 6) = "pnHand" Then
            Dim clickNo = clickedPanel.Name.Substring(6, 1)
            clickCardName = handDataNameList(clickNo - 1)

            gbAllyInfo.Visible = True
        End If

        Dim clickCardData = GetCardDataByName(clickCardName)
        lbSelectCardName.Text = clickCardData.Cardname
        lbSelectCardCost.Text = clickCardData.Cost
        lbSelectCardDefenseMax.Text = "/" & clickCardData.Defense
        lbSelectCardIntellectMax.Text = "/" & clickCardData.Intellect
        txSelectCardDefense.Text = clickCardData.Defense
        txSelectCardIntellect.Text = clickCardData.Intellect
        lbSelectCardAtack.Text = clickCardData.Atack
        lbSelectCardMagic.Text = clickCardData.Magic
        lbSelectCardEffect.Text = clickCardData.Effect
        lbSelectCardLine.Text = clickCardData.Line
        lbSelectCardType.Text = clickCardData.Type

        lbSelectCardSkill1.Text = clickCardData.Skill3
        lbSelectCardSkill2.Text = clickCardData.Skill2
        lbSelectCardSkill3.Text = clickCardData.Skill3

        lbSelectCardFlavor.Text = clickCardData.Flavor
    End Sub

    Private Sub HandCardUpdate()
        For i As Integer = 1 To Constant.HandCardNum

            Dim foundlbHandName() As Control = Me.Controls.Find("lbHandName" & i.ToString(), True)

            Dim targetlbHandName As Control = foundlbHandName(0)
            Dim handLabel As Label = targetlbHandName

            Dim foundlbHandCost() As Control = Me.Controls.Find("lbHandCost" & i.ToString(), True)

            Dim targetlbHandCost As Control = foundlbHandCost(0)
            Dim handCostLabel As Label = targetlbHandCost

            Dim foundlbHandType() As Control = Me.Controls.Find("lbHandType" & i.ToString(), True)

            Dim targetlbHandType As Control = foundlbHandType(0)
            Dim handTypeLabel As Label = targetlbHandType

            Dim foundpnHand() As Control = Me.Controls.Find("pnHand" & i.ToString(), True)

            Dim targetpnHand As Control = foundpnHand(0)
            Dim handPanel As Panel = targetpnHand

            If (handDataNameList(i - 1) <> Nothing) Then
                handLabel.Text = HandData(i - 1).Name
                handCostLabel.Text = HandData(i - 1).Cost
                handTypeLabel.Text = HandData(i - 1).Type
                handLabel.Visible = True
                handCostLabel.Visible = True
                handTypeLabel.Visible = True
                handPanel.Visible = True
            Else
                handLabel.Visible = False
                handCostLabel.Visible = False
                handTypeLabel.Visible = False
                handPanel.Visible = False
            End If
        Next i

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, "B21", {handDataNameList.ToArray.ToList()})
    End Sub
End Class
