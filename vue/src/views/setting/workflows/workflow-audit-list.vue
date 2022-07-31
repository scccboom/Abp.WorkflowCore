<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Form
                    ref="queryForm"
                    :label-width="70"
                    label-position="left"
                    inline
                >
                    <FormItem :label="L('Keyword') + ':'" style="width: 400px">
                        <Input
                            v-model="pagerequest.title"
                            :placeholder="L('Keyword')"
                        />
                    </FormItem>
                    <Button
                        icon="ios-search"
                        type="primary"
                        size="large"
                        class="toolbar-btn"
                        @click="getpage"
                    >
                        {{ L("Find") }}
                    </Button>
                </Form>
                <div class="margin-top-10">
                    <Table
                        :loading="loading"
                        :columns="columns"
                        :no-data-text="L('NoDatas')"
                        :data="list"
                        border
                    />
                    <Page
                        :total="totalCount"
                        :page-size="pageSize"
                        :current="currentPage"
                        show-sizer
                        class-name="fengpage"
                        class="margin-top-10"
                        @on-change="pageChange"
                        @on-page-size-change="pagesizeChange"
                    />
                </div>
            </div>
        </Card>

        <WorkflowDetails
            v-model="showWorkflowDetails"
            :workflow-id="currentSelectedRow.workflowId"
        ></WorkflowDetails>
    </div>
</template>

<script lang="ts">
import { Component, Prop } from "vue-property-decorator";
import AbpBase from "@/lib/abpbase";
import PageRequest from "@/store/entities/page-request";
import WorkflowDetails from "./workflow-details.vue";

class PageWorkflowRequest extends PageRequest {
    title: string;
    auditedMark?: boolean;
}

@Component({
    components: { WorkflowDetails },
})
export default class Workflows extends AbpBase {
    @Prop({ type: Object, default: null })
    request: PageWorkflowRequest;

    pagerequest: PageWorkflowRequest = new PageWorkflowRequest();
    showWorkflowDetails: boolean = false;
    currentSelectedRow: any = {};

    columns = [
        {
            type: "selection",
            width: 60,
            align: "center",
        },
        {
            title: this.L("提交时间"),
            key: "creationTime",
        },
        {
            title: this.L("提交人"),
            key: "userName",
        },
        {
            title: this.L("流程名"),
            key: "title",
        },
        {
            title: this.L("状态"),
            key: "status",
            render: (h: any, params: any) => {
                return h("div", [
                    h(
                        "Tag",
                        {
                            props: {
                                color:
                                    params.row.status == 0
                                        ? "blue"
                                        : params.row.status == 1
                                        ? "cyan"
                                        : "red",
                            },
                        },
                        params.row.status == 0
                            ? "待审核"
                            : params.row.status == 1
                            ? "已同意"
                            : "已拒绝"
                    ),
                ]);
            },
        },

        {
            title: this.L("Actions"),
            key: "Actions",
            width: 250,
            render: (h: any, params: any) => {
                return h("div", [
                    h(
                        "Button",
                        {
                            props: {
                                type: "primary",
                                size: "small",
                            },
                            on: {
                                click: () => {
                                    this.showDetails(params.row);
                                },
                            },
                        },
                        this.L("Details")
                    ),
                ]);
            },
        },
    ];

    get list() {
        return this.$store.state.workflowAuditing.list;
    }

    get loading() {
        return this.$store.state.workflowAuditing.loading;
    }

    get pageSize() {
        return this.$store.state.workflowAuditing.pageSize;
    }
    get totalCount() {
        return this.$store.state.workflowAuditing.totalCount;
    }
    get currentPage() {
        return this.$store.state.workflowAuditing.currentPage;
    }

    pageChange(page: number) {
        this.$store.commit("workflowAuditing/setCurrentPage", page);
        this.getpage();
    }

    pagesizeChange(pagesize: number) {
        this.$store.commit("workflowAuditing/setPageSize", pagesize);
        this.getpage();
    }

    async getpage() {
        this.pagerequest.maxResultCount = this.pageSize;
        this.pagerequest.skipCount = (this.currentPage - 1) * this.pageSize;

        await this.$store.dispatch({
            type: "workflowAuditing/getAll",
            data: this.pagerequest,
        });
    }

    showDetails(row: Workflows) {
        this.currentSelectedRow = row;
        this.showWorkflowDetails = true;
    }

    async created() {
        if (this.request) {
            this.pagerequest.auditedMark = this.request.auditedMark;
        }
        this.getpage();
    }
}
</script>