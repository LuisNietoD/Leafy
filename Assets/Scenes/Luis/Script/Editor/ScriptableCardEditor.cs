using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableCard))]
public class ScriptableCardEditor : Editor
{
    SerializedProperty nameProp;
    SerializedProperty IDProp;
    SerializedProperty artworkProp;
    SerializedProperty backgroundProp;
    SerializedProperty backgroundColorProp;
    SerializedProperty typeProp;

    // Bool section
    SerializedProperty sellableProp;
    SerializedProperty craftableProp;
    SerializedProperty harvestProp;
    SerializedProperty evolveProp;
    // Section
    SerializedProperty priceProp;
    SerializedProperty lifeProp;
    SerializedProperty shakableProp;
    SerializedProperty infiniteProp;
    SerializedProperty dropProp;
    SerializedProperty harvestTimeProp;
    SerializedProperty activatorsProp;
    SerializedProperty inventoryProp;
    SerializedProperty slotProp;
    SerializedProperty timeToCraftProp;
    SerializedProperty recipeProp;
    SerializedProperty evolveTimeProp;
    SerializedProperty evolutionsProp;
    SerializedProperty interfaceProp;
    SerializedProperty interfaceListProp;
    SerializedProperty storeCardProp;
    SerializedProperty storeCardSizeProp;
    SerializedProperty transmuteProp;
    SerializedProperty transmuteListProp;
    SerializedProperty transmuteEnergyProp;
    SerializedProperty transmuteCostProp;
    SerializedProperty transmuteEnergyStoredProp;
    SerializedProperty activableInterfaceProp;

    private Texture2D finalBackground;

    void OnEnable()
    {
        // Initialize serialized properties
        nameProp = serializedObject.FindProperty("name");
        IDProp = serializedObject.FindProperty("ID");
        artworkProp = serializedObject.FindProperty("artwork");
        backgroundProp = serializedObject.FindProperty("background");
        backgroundColorProp = serializedObject.FindProperty("background_color");
        typeProp = serializedObject.FindProperty("type");

        sellableProp = serializedObject.FindProperty("sellable");
        craftableProp = serializedObject.FindProperty("craftable");
        harvestProp = serializedObject.FindProperty("harvestable");

        priceProp = serializedObject.FindProperty("price");
        lifeProp = serializedObject.FindProperty("life");
        shakableProp = serializedObject.FindProperty("shakable");
        infiniteProp = serializedObject.FindProperty("infinite");
        dropProp = serializedObject.FindProperty("drop");
        activatorsProp = serializedObject.FindProperty("activators");
        timeToCraftProp = serializedObject.FindProperty("timeToCraft");
        inventoryProp = serializedObject.FindProperty("inventory");
        slotProp = serializedObject.FindProperty("slot");
        harvestTimeProp = serializedObject.FindProperty("harvestTime");
        recipeProp = serializedObject.FindProperty("recipes");
        evolveProp = serializedObject.FindProperty("evolve");
        evolveTimeProp = serializedObject.FindProperty("evolveTime");
        evolutionsProp = serializedObject.FindProperty("evolutions");
        interfaceProp = serializedObject.FindProperty("interfaces");
        interfaceListProp = serializedObject.FindProperty("interfaceList");
        storeCardProp = serializedObject.FindProperty("storeCard");
        storeCardSizeProp = serializedObject.FindProperty("inventorySize");
        transmuteProp = serializedObject.FindProperty("transmute");
        transmuteListProp = serializedObject.FindProperty("transmuteList");
        transmuteEnergyProp = serializedObject.FindProperty("requireEnergy");
        transmuteCostProp = serializedObject.FindProperty("energyPerCraft");
        transmuteEnergyStoredProp = serializedObject.FindProperty("maxEnergyStored");
        activableInterfaceProp = serializedObject.FindProperty("activableInterface");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Display the standard ScriptableObject fields
        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(IDProp);
        EditorGUILayout.PropertyField(artworkProp);
        EditorGUILayout.PropertyField(backgroundProp);
        EditorGUILayout.PropertyField(backgroundColorProp);
        EditorGUILayout.PropertyField(typeProp);

        EditorGUILayout.PropertyField(sellableProp);
        EditorGUILayout.PropertyField(craftableProp);
        EditorGUILayout.PropertyField(harvestProp);
        EditorGUILayout.PropertyField(inventoryProp);
        EditorGUILayout.PropertyField(evolveProp);
        EditorGUILayout.PropertyField(interfaceProp);
        EditorGUILayout.PropertyField(transmuteProp);
        EditorGUILayout.PropertyField(activableInterfaceProp);

        // Display sell information
        if (sellableProp.boolValue)
        {
            EditorGUILayout.PropertyField(priceProp, new GUIContent("Price"));
        }

        // Display craft information
        if (craftableProp.boolValue)
        {
            EditorGUILayout.PropertyField(timeToCraftProp, new GUIContent("Time to Craft"));
            EditorGUILayout.PropertyField(recipeProp, new GUIContent("Recipe"));
        }

        // Check if infinite is true, then show life
        if (harvestProp.boolValue)
        {
            EditorGUILayout.PropertyField(infiniteProp, new GUIContent("Infinite"));
            if (!infiniteProp.boolValue)
                EditorGUILayout.PropertyField(lifeProp, new GUIContent("Life"));
            EditorGUILayout.PropertyField(shakableProp, new GUIContent("Shakable"));
            EditorGUILayout.PropertyField(storeCardProp, new GUIContent("Store card"));
            EditorGUILayout.PropertyField(storeCardSizeProp, new GUIContent("Inventory Size"));
            EditorGUILayout.PropertyField(harvestTimeProp, new GUIContent("Harvest Time"));
            EditorGUILayout.PropertyField(dropProp, new GUIContent("Drop"));
            EditorGUILayout.PropertyField(activatorsProp, new GUIContent("Activators"));
        }

        if (inventoryProp.boolValue)
        {
            EditorGUILayout.PropertyField(slotProp, new GUIContent("Slots"));
        }

        if (evolveProp.boolValue)
        {
            EditorGUILayout.PropertyField(evolveTimeProp, new GUIContent("Evolve Time"));
            EditorGUILayout.PropertyField(evolutionsProp, new GUIContent("Evolutions"));
        }

        if (interfaceProp.boolValue)
        {
            EditorGUILayout.PropertyField(interfaceListProp, new GUIContent("Interface List"));
        }
        
        if (transmuteProp.boolValue)
        {
            EditorGUILayout.PropertyField(transmuteEnergyProp, new GUIContent("Energy required"));
            if (transmuteEnergyProp.boolValue)
            {
                EditorGUILayout.PropertyField(transmuteCostProp, new GUIContent("Transmute cost"));
                EditorGUILayout.PropertyField(transmuteEnergyStoredProp, new GUIContent("Max Energy stored"));
            }

            EditorGUILayout.PropertyField(transmuteListProp, new GUIContent("Transmute List"));
        }

        // Custom preview area
        EditorGUILayout.Space();

        // Combine name and sprite for preview
        string cardPreviewText = nameProp.stringValue;
        Sprite cardPreviewSprite = artworkProp.objectReferenceValue as Sprite;

        if (cardPreviewSprite != null)
        {
            // Display the preview with name and sprite
            GUILayout.Label("Card Preview", EditorStyles.boldLabel);

            // Get the icon texture
            Texture2D iconTexture = AssetPreview.GetAssetPreview(artworkProp.objectReferenceValue);

            // Get the background texture
            Texture2D backgroundTexture = AssetPreview.GetAssetPreview(backgroundProp.objectReferenceValue);

            // Calculate position for centering
            float centerX = (EditorGUILayout.GetControlRect(GUILayout.Width(200), GUILayout.Height(300)).width - 200) * 0.5f;
            Rect centeredRect = GUILayoutUtility.GetLastRect();
            centeredRect.x += centerX;

            // Create a new texture with the blended background color
            finalBackground = GetBlendedBackground(backgroundTexture, backgroundColorProp.colorValue);

            // Draw the final blended background
            GUI.DrawTexture(centeredRect, finalBackground);

            // Calculate a scaled-down size for the icon (assuming the icon is square)
            float scaleDownFactor = 0.5f;
            float scaledIconSize = Mathf.Min(200, 300) * scaleDownFactor;

            // Calculate centered position for the scaled icon
            float iconX = centeredRect.x + (centeredRect.width - scaledIconSize) * 0.5f;
            float iconY = centeredRect.y + (centeredRect.height - scaledIconSize) * 0.5f;

            // Draw scaled-down icon on top of the background
            Rect iconRect = new Rect(iconX, iconY, scaledIconSize, scaledIconSize);
            GUI.DrawTexture(iconRect, iconTexture);

            // Display text on the texture using Handles.Label
            string labelText = nameProp.stringValue;
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.normal.textColor = Color.black;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 16; // Increased font size for more boldness

            Vector2 labelSize = labelStyle.CalcSize(new GUIContent(labelText));
            float labelX = centeredRect.x + (centeredRect.width - labelSize.x) * 0.5f;
            float labelY = centeredRect.y + centeredRect.height * 0.22f; // Centered at 5% of the height from the top
            Vector3 labelPosition = new Vector3(labelX, labelY, 0f);

            Handles.Label(labelPosition, labelText, labelStyle);
            
            // Display ID in white below the name
            string idText = "ID: " + IDProp.intValue;
            GUIStyle idStyle = new GUIStyle(EditorStyles.label);
            idStyle.normal.textColor = Color.white;
            idStyle.fontStyle = FontStyle.Bold;
            idStyle.fontSize = 30; // Adjust font size as needed

            Vector2 idSize = idStyle.CalcSize(new GUIContent(idText));
            float idX = centeredRect.x + (centeredRect.width - idSize.x) * 0.5f;
            float idY = centeredRect.y + centeredRect.height * 0.92f;
            Vector3 idPosition = new Vector3(idX, idY, 0f);

            Handles.Label(idPosition, idText, idStyle);
        }

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
    
    private Texture2D GetBlendedBackground(Texture2D backgroundTexture, Color backgroundColor)
    {
        int width = backgroundTexture.width;
        int height = backgroundTexture.height;

        // Get the pixels of the background texture
        Color[] backgroundPixels = backgroundTexture.GetPixels();

        // Create a new array for the final pixels
        Color[] finalPixels = new Color[width * height];

        for (int i = 0; i < finalPixels.Length; i++)
        {
            // Check for almost white in the background
            if (backgroundPixels[i].r >= 0.9f && backgroundPixels[i].a > 0.2f)
            {
                // Blend with the specified backgroundColor
                finalPixels[i] = backgroundColor;
            }
            else if (backgroundPixels[i].a > 0.2f)
            {
                finalPixels[i] = backgroundPixels[i];
            }
        }

        // Create a new texture with the blended pixels
        Texture2D finalBackground = new Texture2D(width, height);
        finalBackground.SetPixels(finalPixels);
        finalBackground.Apply();
        return finalBackground;
    }
    
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        ScriptableCard soCard = (ScriptableCard)target;

        Texture2D cardIcon = soCard.GetCardIcon(); 
        
        if (cardIcon != null)
        {
            Texture2D resizedIcon = ResizeTexture(cardIcon, width, height);

            return resizedIcon;
        }

        return null;
    }
    
    private Texture2D ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
        Graphics.Blit(sourceTexture, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight);
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        DestroyImmediate(rt);

        return result;
    }
}
