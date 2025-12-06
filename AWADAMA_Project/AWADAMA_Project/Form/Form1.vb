Imports System.Reflection.Metadata

Public Class Form1
    Dim HandData As List(Of HandDataDto) = New List(Of HandDataDto)
    Dim GraveData As List(Of CardDataDto) = New List(Of CardDataDto)
    Dim SelectionCard As CardDataDto = New CardDataDto()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AllCardData.GetData()
    End Sub

    Private Sub DeckShuffle_Button_Click(sender As Object, e As EventArgs) Handles btShuffleButton.Click

        Dim result As MsgBoxResult = MsgBox("デッキをシャッフルしますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")

        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim deck As IList(Of IList(Of Object))
        Dim archive As IList(Of IList(Of Object))

        deck = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_DECK, "B2:B" & Constant.MAX_CARD_CELL)

        archive = Common.ListShuffle(Of IList(Of Object))(deck)

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "B2:B" & Constant.MAX_CARD_CELL, archive)

    End Sub

    Private Sub Deck_Click(sender As Object, e As EventArgs) Handles btDeck.Click

        If HandData.Count >= HandCardNum Then
            Exit Sub
        End If

        Dim archive As IList(Of IList(Of Object))

        archive = GetGoogleSheetData(MAIN_SHEET_ID, MAIN_SHEET_NAME_ARCHIVE, "A2:C" & MAX_CARD_CELL)

        Dim index = 1
        Dim drawIndex = 1
        Dim disCartCount = 1
        Dim isGet = False
        For Each card In archive
            If card.Count = 2 Then
                If Not isGet Then
                    HandData.Add(New HandDataDto(card(0), card(1), ""))
                    isGet = True
                    drawIndex = index
                End If
            Else
                If card(2).ToString = "1" Then
                    disCartCount += 1
                End If
            End If
            index += 1
        Next

        SetGoogleSheetData(MAIN_SHEET_ID, MAIN_SHEET_NAME_ARCHIVE, "C" & drawIndex + 1, "1")

        btDeck.Text = (MAX_DECK_COUNT - disCartCount).ToString

        HandCardUpdate()

        If HandData.Count >= HandCardNum Then
            lbHandMaxSign.Visible = True
        End If
    End Sub

    Private Sub pnCard_Click(sender As Object, e As EventArgs) Handles pnHand1.Click, pnHand2.Click, pnHand3.Click, pnHand4.Click, pnHand5.Click, pnHand6.Click
        Dim clickedPanel = TryCast(sender, Panel)

        pnSelectPanelColor.Left = clickedPanel.Location.X - 5
        pnSelectPanelColor.Top = clickedPanel.Location.Y - 5
        pnSelectPanelColor.Visible = True

        btDiscard.Visible = False

        SelectionCard = Nothing

        '手札選択時
        If clickedPanel.Name.Substring(0, 6) = "pnHand" Then
            Dim clickNo = clickedPanel.Name.Substring(6, 1)
            Dim clickCardName = HandData(clickNo - 1).Name

            gbAllyInfo.Visible = True
            btDiscard.Visible = True

            Dim clickCardData = GetCardDataByName(clickCardName)
            SetAllySelectionData(clickCardData)

            SelectionCard = clickCardData
            SelectionCard.DeckNo = HandData(clickNo - 1).No
        End If
    End Sub

    Private Sub btDiscard_Click(sender As Object, e As EventArgs) Handles btDiscard.Click
        Dim result As MsgBoxResult = MsgBox("選択中のカードを墓地に送りますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")
        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        pnSelectPanelColor.Visible = False

        For Each no As String In HandData.Select(Function(x) x.No).ToList()
            If no = SelectionCard.DeckNo Then
                GraveData.Add(GetCardDataByName(SelectionCard.Name))
                HandData.Remove(HandData.Find(Function(x) x.No = SelectionCard.DeckNo))
                HandCardUpdate()
                Exit For
            End If
        Next

        gbAllyInfo.Visible = False
        btDiscard.Visible = False
        lbHandMaxSign.Visible = False
    End Sub

    Private Sub SetAllySelectionData(selectCardData As CardDataDto)
        lbSelectCardName.Text = selectCardData.Cardname
        lbSelectCardCost.Text = selectCardData.Cost
        lbSelectCardDefenseMax.Text = "/" & selectCardData.Defense
        lbSelectCardIntellectMax.Text = "/" & selectCardData.Intellect
        txSelectCardDefense.Text = selectCardData.Defense
        txSelectCardIntellect.Text = selectCardData.Intellect
        lbSelectCardAtack.Text = selectCardData.Atack
        lbSelectCardMagic.Text = selectCardData.Magic
        lbSelectCardEffect.Text = selectCardData.Effect
        lbSelectCardLine.Text = selectCardData.Line
        lbSelectCardType.Text = selectCardData.Type

        lbSelectCardSkill1.Text = selectCardData.Skill3
        lbSelectCardSkill2.Text = selectCardData.Skill2
        lbSelectCardSkill3.Text = selectCardData.Skill3

        lbSelectCardFlavor.Text = selectCardData.Flavor
    End Sub

    Private Sub HandCardUpdate()

        Dim handDataNameList = New List(Of String)

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

            If (i - 1 < HandData.Count) Then

                handDataNameList.Add(HandData(i - 1).Name)

                Dim CardData = GetCardDataByName(HandData(i - 1).Name)
                handLabel.Text = CardData.Name
                handCostLabel.Text = CardData.Cost
                handTypeLabel.Text = CardData.Type
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

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, "B21", {handDataNameList.ToArray()})
    End Sub
End Class
