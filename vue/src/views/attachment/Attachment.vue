<template>
    <div>
        <Upload :action="singleUploadUrl">
            <Button icon="ios-cloud-upload-outline">Upload Single File</Button>
        </Upload>
        <Upload
            name="formFile"
            :action="wrapperUploadUrl"
            :data="{ tag: 'test' }"
        >
            <Button icon="ios-cloud-upload-outline">Upload Wrapper File</Button>
        </Upload>
        <Upload name="fileCollection" :action="collectionUploadUrl" multiple>
            <Button icon="ios-cloud-upload-outline">
                Upload File Collection
            </Button>
        </Upload>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";

@Component
export default class Attachment extends AbpBase {
    private host: string = "http://localhost:21021";

    public get singleUploadUrl(): string {
        return `${this.host}/api/services/app/Attachment/SingleUpload`;
    }

    public get wrapperUploadUrl(): string {
        return `${this.host}/api/services/app/Attachment/Upload`;
    }

    public get collectionUploadUrl(): string {
        return `${this.host}/api/services/app/Attachment/UploadList`;
    }
}
</script>
